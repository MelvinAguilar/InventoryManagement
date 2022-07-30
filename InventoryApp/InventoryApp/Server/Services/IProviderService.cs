namespace InventoryApp.Server.Services
{
    public interface IProviderService
    {
        Task<ServerResponse<IEnumerable<Provider>>> GetAllProviders();
        Task<ServerResponse<Provider>> GetProviderById(int id);
        Task<ServerResponse<Provider>> AddProvider(Provider provider);
        Task<ServerResponse<bool>> UpdateProvider(int id, Provider provider);
        Task<ServerResponse<bool>> DeleteProvider(int id);
    }
}