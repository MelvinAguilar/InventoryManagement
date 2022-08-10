namespace InventoryApp.Client.Services
{
    public interface ICustomerService
    {
        Task<ServiceResponse<List<GetCustomerDto>>> GetCustomers();
        Task<ServiceResponse<GetCustomerDto>> GetCustomer(int id);
        Task<ServiceResponse<GetCustomerDto>> AddCustomer(AddCustomerDto request);
        Task<ServiceResponse<bool>> UpdateCustomer(UpdateCustomerDto request);
        Task<ServiceResponse<bool>> DeleteCustomer(int id); 
    }
}