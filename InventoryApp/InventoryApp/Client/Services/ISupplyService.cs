namespace InventoryApp.Client.Services
{
    public interface ISupplyService
    {
        Task<ServiceResponse<List<GetSupplyDto>>> GetSupplies();
        Task<ServiceResponse<GetSupplyDto>> GetSupply(int id);
        Task<ServiceResponse<GetSupplyDto>> AddSupply(AddSupplyDto request);
        Task<ServiceResponse<bool>> UpdateSupply(UpdateSupplyDto request);
    }
}