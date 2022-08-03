using InventoryApp.Server.Dtos.PurchaseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController] 
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetPurchaseDto>>>> GetPurchases()
        {
            return HandleResponse(await _purchaseService.GetAllPurchases());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetPurchaseDto>>> GetPurchase(int id)
        {
            return HandleResponse(await _purchaseService.GetPurchaseById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetPurchaseDto>>> PostPurchase(AddPurchaseDto purchase)
        {
            return HandleResponse(await _purchaseService.AddPurchase(purchase));
        }

        [Authorize (Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> PutPurchase(int id, UpdatePurchaseDto purchase)
        {
            return HandleResponse(await _purchaseService.UpdatePurchase(id, purchase));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServiceResponse<T>> HandleResponse<T> (ServiceResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}