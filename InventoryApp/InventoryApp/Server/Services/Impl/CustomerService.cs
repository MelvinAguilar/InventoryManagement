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
            var newCustomer = _mapper.Map<Customer>(customer);
            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return new ServerResponse<GetCustomerDto> { Data = _mapper.Map<GetCustomerDto>(newCustomer) };
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
            }
            else
            {
                try
                {
                    // TODO: Update only a part of the entity
                    var updatedCustomer = _mapper.Map<Customer>(customer);
                    _context.Entry(updatedCustomer).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    
                    response.Data = true;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    response.Success = false;
                    if (!CustomerExists(id))
                        response.Message = "Customer not found";
                    else
                        response.Message = "Error updating category: " + e.Message; 
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
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                
                response.Data = true;
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
    }
}