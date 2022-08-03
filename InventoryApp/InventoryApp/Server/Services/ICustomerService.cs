using InventoryApp.Server.Dtos.CustomerDtos;

namespace InventoryApp.Server.Services
{
    public interface ICustomerService
    {
        Task<ServiceResponse<IEnumerable<GetCustomerDto>>> GetAllCustomers();
        Task<ServiceResponse<GetCustomerDto>> GetCustomerById(int id);
        Task<ServiceResponse<GetCustomerDto>> AddCustomer(AddCustomerDto customer);
        Task<ServiceResponse<bool>> UpdateCustomer(int id, UpdateCustomerDto customer);
        Task<ServiceResponse<bool>> DeleteCustomer(int id);
    }
}