using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController] 
    public class SupplyController : ControllerBase
    {
        private readonly ISupplyService _supplyService;

        public SupplyController(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetSupplyDto>>>> GetSupplies()
        {
            return HandleResponse(await _supplyService.GetAllSupplies());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetSupplyDto>>> GetSupply(int id)
        {
            return HandleResponse(await _supplyService.GetSupplyById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetSupplyDto>>> PostSupply(AddSupplyDto supply)
        {
            return HandleResponse(await _supplyService.AddSupply(supply));
        }

        [Authorize (Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> PutSupply(int id, UpdateSupplyDto supply)
        {
            return HandleResponse(await _supplyService.UpdateSupply(id, supply));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServiceResponse<T>> HandleResponse<T> (ServiceResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}