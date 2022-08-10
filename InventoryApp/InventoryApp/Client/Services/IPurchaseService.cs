namespace InventoryApp.Client.Services
{
    public interface IPurchaseService
    {
        Task<ServiceResponse<List<GetPurchaseDto>>> GetPurchases();
        Task<ServiceResponse<GetPurchaseDto>> GetPurchase(int id);
        Task<ServiceResponse<GetPurchaseDto>> AddPurchase(AddPurchaseDto request);
        Task<ServiceResponse<bool>> UpdatePurchase(UpdatePurchaseDto request);
    }
}