using InventoryApp.Server.Dtos.CustomerDtos;

namespace InventoryApp.Server.Services
{
    public interface ICustomerService
    {
        Task<ServerResponse<IEnumerable<GetCustomerDto>>> GetAllCustomers();
        Task<ServerResponse<GetCustomerDto>> GetCustomerById(int id);
        Task<ServerResponse<GetCustomerDto>> AddCustomer(AddCustomerDto customer);
        Task<ServerResponse<bool>> UpdateCustomer(int id, UpdateCustomerDto customer);
        Task<ServerResponse<bool>> DeleteCustomer(int id);
    }
}