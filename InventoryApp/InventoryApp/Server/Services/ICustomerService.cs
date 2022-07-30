namespace InventoryApp.Server.Services
{
    public interface ICustomerService
    {
        Task<ServerResponse<IEnumerable<Customer>>> GetAllCustomers();
        Task<ServerResponse<Customer>> GetCustomerById(int id);
        Task<ServerResponse<Customer>> AddCustomer(Customer customer);
        Task<ServerResponse<bool>> UpdateCustomer(int id, Customer customer);
        Task<ServerResponse<bool>> DeleteCustomer(int id);
    }
}