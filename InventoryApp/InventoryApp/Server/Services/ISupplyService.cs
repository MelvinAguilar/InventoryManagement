using InventoryApp.Server.Dtos.SupplyDtos;

namespace InventoryApp.Server.Services
{
    public interface ISupplyService
    {
        Task<ServerResponse<IEnumerable<GetSupplyDto>>> GetAllSupplies();
        Task<ServerResponse<GetSupplyDto>> GetSupplyById(int id);
        Task<ServerResponse<GetSupplyDto>> AddSupply(AddSupplyDto supply);
        Task<ServerResponse<bool>> UpdateSupply(int id, UpdateSupplyDto supply);
    }
}