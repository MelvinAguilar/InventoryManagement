using InventoryApp.Server.Dtos.ProviderDtos;

namespace InventoryApp.Server.Services
{
    public interface IProviderService
    {
        Task<ServiceResponse<IEnumerable<GetProviderDto>>> GetAllProviders();
        Task<ServiceResponse<GetProviderDto>> GetProviderById(int id);
        Task<ServiceResponse<GetProviderDto>> AddProvider(AddProviderDto provider);
        Task<ServiceResponse<bool>> UpdateProvider(int id, UpdateProviderDto provider);
        Task<ServiceResponse<bool>> DeleteProvider(int id);
    }
}