using InventoryApp.Server.Dtos.ProviderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]    
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetProviderDto>>>> GetProviders()
        {
            return HandleResponse(await _providerService.GetAllProviders());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetProviderDto>>> GetProvider(int id)
        {
            return HandleResponse(await _providerService.GetProviderById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetProviderDto>>> PostProvider(AddProviderDto provider)
        {
            return HandleResponse(await _providerService.AddProvider(provider));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> PutProvider(int id, UpdateProviderDto provider)
        {
            return HandleResponse(await _providerService.UpdateProvider(id, provider));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProvider(int id)
        {
            return HandleResponse(await _providerService.DeleteProvider(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServiceResponse<T>> HandleResponse<T> (ServiceResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}