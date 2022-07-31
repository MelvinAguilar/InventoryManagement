using InventoryApp.Server.Dtos.ProviderDtos;

namespace InventoryApp.Server.Services
{
    public interface IProviderService
    {
        Task<ServerResponse<IEnumerable<GetProviderDto>>> GetAllProviders();
        Task<ServerResponse<GetProviderDto>> GetProviderById(int id);
        Task<ServerResponse<GetProviderDto>> AddProvider(AddProviderDto provider);
        Task<ServerResponse<bool>> UpdateProvider(int id, UpdateProviderDto provider);
        Task<ServerResponse<bool>> DeleteProvider(int id);
    }
}