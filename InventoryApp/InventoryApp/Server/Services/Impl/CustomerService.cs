using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        public readonly inventory_managementContext _context;

        public CustomerService(inventory_managementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>List of customers wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<Customer>>> GetAllCustomers()
        {
            var response = new ServerResponse<IEnumerable<Customer>>();
            var customers = await _context.Customers.ToListAsync();

            if (customers == null)
            {
                response.Success = false;
                response.Message = "No customers found";
            }
            else
            {
                response.Data = customers;
            }

            return response;
        }

        /// <summary>
        /// Get customer by id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <returns>Customer wrapped in a response</returns>
        public async Task<ServerResponse<Customer>> GetCustomerById(int id)
        {
            var response = new ServerResponse<Customer>();
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                response.Success = false;
                response.Message = "Customer not found";
            }
            else
            {
                response.Data = customer;
            }

            return response;
        }

        /// <summary>
        /// Add new customer into database
        /// </summary>
        /// <param name="customer">Customer to insert into database</param>
        /// <returns>Added category wrapped in a response</returns>  
        public async Task<ServerResponse<Customer>> AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return new ServerResponse<Customer> { Data = customer };
        }

        /// <summary>
        /// Update customer in database
        /// </summary>
        /// <param name="id">Customer id to update</param>
        /// <param name="customer">Customer to update in database</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdateCustomer(int id, Customer customer)
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
                    _context.Entry(customer).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    
                    response.Data = true;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    response.Success = false;
                    if (!CustomerExists(customer.Id))
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