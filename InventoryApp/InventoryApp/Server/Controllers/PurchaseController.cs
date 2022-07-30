using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class PurchaseController : ControllerBase
    {
        public readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<ActionResult<ServerResponse<IEnumerable<Purchase>>>> GetPurchases()
        {
            return HandleResponse(await _purchaseService.GetAllPurchases());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<Purchase>>> GetPurchase(int id)
        {
            return HandleResponse(await _purchaseService.GetPurchaseById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServerResponse<Purchase>>> PostPurchase(Purchase purchase)
        {
            return HandleResponse(await _purchaseService.AddPurchase(purchase));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> PutPurchase(int id, Purchase purchase)
        {
            return HandleResponse(await _purchaseService.UpdatePurchase(id, purchase));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> DeletePurchase(int id)
        {
            return HandleResponse(await _purchaseService.DeletePurchase(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServerResponse<T>> HandleResponse<T> (ServerResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}