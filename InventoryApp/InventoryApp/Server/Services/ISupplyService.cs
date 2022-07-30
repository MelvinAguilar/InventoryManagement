namespace InventoryApp.Server.Services
{
    public interface ISupplyService
    {
        Task<ServerResponse<IEnumerable<Supply>>> GetAllSupplies();
        Task<ServerResponse<Supply>> GetSupplyById(int id);
        Task<ServerResponse<Supply>> AddSupply(Supply supply);
        Task<ServerResponse<bool>> UpdateSupply(int id, Supply supply);
        Task<ServerResponse<bool>> DeleteSupply(int id);
    }
}