namespace InventoryApp.Server.Services
{
    public interface IPurchaseService
    {
        Task<ServerResponse<IEnumerable<Purchase>>> GetAllPurchases();
        Task<ServerResponse<Purchase>> GetPurchaseById(int id);
        Task<ServerResponse<Purchase>> AddPurchase(Purchase purchase);
        Task<ServerResponse<bool>> UpdatePurchase(int id, Purchase purchase);
        Task<ServerResponse<bool>> DeletePurchase(int id);
    }
}