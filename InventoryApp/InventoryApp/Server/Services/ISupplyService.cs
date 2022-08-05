namespace InventoryApp.Server.Services
{
    public interface ISupplyService
    {
        Task<ServiceResponse<IEnumerable<GetSupplyDto>>> GetAllSupplies();
        Task<ServiceResponse<GetSupplyDto>> GetSupplyById(int id);
        Task<ServiceResponse<GetSupplyDto>> AddSupply(AddSupplyDto supply);
        Task<ServiceResponse<bool>> UpdateSupply(int id, UpdateSupplyDto supply);
    }
}