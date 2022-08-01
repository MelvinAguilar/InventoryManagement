using AutoMapper;
using InventoryApp.Server.Dtos.CustomerDtos;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        public readonly inventory_managementContext _context;
        public readonly IMapper _mapper;

        public CustomerService(inventory_managementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>List of customers wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<GetCustomerDto>>> GetAllCustomers()
        {
            var response = new ServerResponse<IEnumerable<GetCustomerDto>>();
            var customers = await _context.Customers.ToListAsync();

            if (customers == null)
            {
                response.Success = false;
                response.Message = "No customers found";
            }
            else
            {
                response.Data = _mapper.Map<IEnumerable<GetCustomerDto>>(customers);
            }

            return response;
        }

        /// <summary>
        /// Get customer by id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <returns>Customer wrapped in a response</returns>
        public async Task<ServerResponse<GetCustomerDto>> GetCustomerById(int id)
        {
            var response = new ServerResponse<GetCustomerDto>();
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                response.Success = false;
                response.Message = "Customer not found";
            }
            else
            {
                response.Data = _mapper.Map<GetCustomerDto>(customer);
            }

            return response;
        }

        /// <summary>
        /// Add new customer into database
        /// </summary>
        /// <param name="customer">Customer to insert into database</param>
        /// <returns>Added category wrapped in a response</returns>  
        public async Task<ServerResponse<GetCustomerDto>> AddCustomer(AddCustomerDto customer)
        {
            var response = new ServerResponse<GetCustomerDto>();
            // Try catch block to catch any errors that may occur while inserting into database
            try 
            {
                var newCustomer = _mapper.Map<Customer>(customer);
                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCustomerDto>(newCustomer);
            } 
            catch (DbUpdateException e) 
            {
                // If enter here, it means that the phone number already exists in the database
                response.Success = false;
                if (CustomerExists(customer.PhoneNumber))
                    response.Message = "Customer with phone number " + customer.PhoneNumber + " already exists";
                else
                    response.Message = "Error adding category: " + e.Message;
            }

            return response;
        }

        /// <summary>
        /// Update customer in database
        /// </summary>
        /// <param name="id">Customer id to update</param>
        /// <param name="customer">Customer to update in database</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdateCustomer(int id, UpdateCustomerDto customer)
        {
            var response = new ServerResponse<bool>();
            if (id != customer.Id)
            {
                response.Success = false;
                response.Message = "Category id mismatch";
                return response;
            }

            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
            { 
                response.Success = false;
                response.Message = "Customer not found";
            } 
            else 
            {
                try
                {
                    // Update only a few properties of the customer in database
                    // I dont want to update the DateCreated property
                    // that's why I don't use: "_context.Entry(customer).State = EntityState.Modified;"
                    _context.Customers.Attach(existingCustomer);
                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LastName = customer.LastName;
                    existingCustomer.PhoneNumber = customer.PhoneNumber;
                    existingCustomer.DateModified = DateTime.Now;

                    await _context.SaveChangesAsync();
                    response.Data = true;
                }
                catch (DbUpdateException e)
                {
                    // If enter here, it means that the phone number is already in use
                    response.Success = false;
                    if (CustomerExists(customer.PhoneNumber))
                        response.Message = "Customer with phone number " + customer.PhoneNumber + " already exists";
                    else
                        response.Message = "Error adding category: " + e.Message;
                }               
            }

            return response;
        }

        /// <summary>
        /// Delete customer from database
        /// </summary>
        /// <param name="id">Customer id to delete</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> DeleteCustomer(int id)
        {
            var response = new ServerResponse<bool>();
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                response.Success = false;
                response.Message = "Customer not found";
            }
            else
            {
                // Try catch block for deleting customer with related purchase orders
                try
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();

                    response.Data = true;
                }
                catch (DbUpdateException)
                {
                    response.Success = false;
                    response.Message = "Error deleting customer: Customer has related purchase orders";
                }   
            }

            return response;
        }

        /// <summary>
        /// Check if customer exists in database
        /// </summary>
        /// <param name="id">Customer id to check</param>
        /// <returns>True if customer exists, false otherwise</returns>
        private bool CustomerExists(int id) 
            => (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();

        /// <summary>
        /// Check if customer exists in database
        /// </summary>
        /// <param name="phoneNumber">Customer phone number to check</param>
        /// <returns>True if customer exists, false otherwise</returns>
        private bool CustomerExists(string phoneNumber) 
            => (_context.Customers?.Any(c => c.PhoneNumber == phoneNumber)).GetValueOrDefault();
    }
}