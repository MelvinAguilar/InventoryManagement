using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ProviderController : ControllerBase
    {
        public readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet]
        public async Task<ActionResult<ServerResponse<IEnumerable<Provider>>>> GetProviders()
        {
            return HandleResponse(await _providerService.GetAllProviders());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<Provider>>> GetProvider(int id)
        {
            return HandleResponse(await _providerService.GetProviderById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServerResponse<Provider>>> PostProvider(Provider provider)
        {
            return HandleResponse(await _providerService.AddProvider(provider));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> PutProvider(int id, Provider provider)
        {
            return HandleResponse(await _providerService.UpdateProvider(id, provider));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> DeleteProvider(int id)
        {
            return HandleResponse(await _providerService.DeleteProvider(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServerResponse<T>> HandleResponse<T> (ServerResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}