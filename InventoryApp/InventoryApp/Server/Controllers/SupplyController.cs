using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class SupplyController : ControllerBase
    {
        public readonly ISupplyService _supplyService;

        public SupplyController(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }

        [HttpGet]
        public async Task<ActionResult<ServerResponse<IEnumerable<Supply>>>> GetSupplies()
        {
            return HandleResponse(await _supplyService.GetAllSupplies());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<Supply>>> GetSupply(int id)
        {
            return HandleResponse(await _supplyService.GetSupplyById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServerResponse<Supply>>> PostSupply(Supply supply)
        {
            return HandleResponse(await _supplyService.AddSupply(supply));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> PutSupply(int id, Supply supply)
        {
            return HandleResponse(await _supplyService.UpdateSupply(id, supply));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> DeleteSupply(int id)
        {
            return HandleResponse(await _supplyService.DeleteSupply(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServerResponse<T>> HandleResponse<T> (ServerResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}