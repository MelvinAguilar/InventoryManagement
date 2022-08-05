using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private readonly inventory_managementContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerService(inventory_managementContext context, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>List of customers wrapped in a service response</returns>
        public async Task<ServiceResponse<IEnumerable<GetCustomerDto>>> GetAllCustomers()
        {
            var response = new ServiceResponse<IEnumerable<GetCustomerDto>>();
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
        /// <returns>Customer wrapped in a service response</returns>
        public async Task<ServiceResponse<GetCustomerDto>> GetCustomerById(int id)
        {
            var response = new ServiceResponse<GetCustomerDto>();
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
        /// <returns>Added category wrapped in a service response</returns>  
        public async Task<ServiceResponse<GetCustomerDto>> AddCustomer(AddCustomerDto customer)
        {
            var response = new ServiceResponse<GetCustomerDto>();
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
                if (CustomerExists(customer.FirstName, customer.PhoneNumber))
                    response.Message = "Customer with the same name and phone number already exists";
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
        /// <returns>Success or error message in service response</returns>
        public async Task<ServiceResponse<bool>> UpdateCustomer(int id, UpdateCustomerDto customer)
        {
            var response = new ServiceResponse<bool>();
            if (id != customer.Id)
            {
                response.Success = false;
                response.Message = "Customer id mismatch";
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
                    if (CustomerExists(customer.FirstName, customer.PhoneNumber))
                        response.Message = "Customer with the same name and phone number already exists";
                    else
                        response.Message = "Error adding customer: " + e.Message;
                }               
            }

            return response;
        }

        /// <summary>
        /// Delete customer from database
        /// </summary>
        /// <param name="id">Customer id to delete</param>
        /// <returns>Success or error message in service response</returns>
        public async Task<ServiceResponse<bool>> DeleteCustomer(int id)
        {
            var response = new ServiceResponse<bool>();
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
        /// <param name="name">Customer name to check</param>
        /// <param name="phoneNumber">Customer phone number to check</param>
        /// <returns>True if customer exists, false otherwise</returns>
        private bool CustomerExists(string name, string phoneNumber) 
            => (_context.Customers?.Any(c => c.FirstName == name && c.PhoneNumber == phoneNumber)).GetValueOrDefault();

        /// <summary>
        /// Get the ID of the authenticated employee
        /// </summary>
        /// <returns>Employee ID</returns>
        /* This method may be used in the future to auditory the employee who submits request
           to change a customer*/
        private int GetAuthenticatedEmployeeId()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context found");
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}