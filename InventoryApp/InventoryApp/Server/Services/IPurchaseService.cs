namespace InventoryApp.Server.Services
{
    public interface IPurchaseService
    {
        Task<ServiceResponse<IEnumerable<GetPurchaseDto>>> GetAllPurchases();
        Task<ServiceResponse<GetPurchaseDto>> GetPurchaseById(int id);
        Task<ServiceResponse<GetPurchaseDto>> AddPurchase(AddPurchaseDto purchase);
        Task<ServiceResponse<bool>> UpdatePurchase(int id, UpdatePurchaseDto purchase);
    }
}