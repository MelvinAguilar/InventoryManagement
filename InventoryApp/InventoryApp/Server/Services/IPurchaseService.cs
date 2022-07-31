using InventoryApp.Server.Dtos.PurchaseDtos;

namespace InventoryApp.Server.Services
{
    public interface IPurchaseService
    {
        Task<ServerResponse<IEnumerable<GetPurchaseDto>>> GetAllPurchases();
        Task<ServerResponse<GetPurchaseDto>> GetPurchaseById(int id);
        Task<ServerResponse<GetPurchaseDto>> AddPurchase(AddPurchaseDto purchase);
        Task<ServerResponse<bool>> UpdatePurchase(int id, UpdatePurchaseDto purchase);
    }
}