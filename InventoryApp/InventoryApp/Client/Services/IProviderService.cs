namespace InventoryApp.Client.Services
{
    public interface IProviderService
    {
        Task<ServiceResponse<List<GetProviderDto>>> GetProviders();
        Task<ServiceResponse<GetProviderDto>> GetProvider(int id);
        Task<ServiceResponse<GetProviderDto>> AddProvider(AddProviderDto request);
        Task<ServiceResponse<bool>> UpdateProvider(UpdateProviderDto request);
        Task<ServiceResponse<bool>> DeleteProvider(int id);
    }
}