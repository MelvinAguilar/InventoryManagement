using InventoryApp.Server.Dtos.CustomerDtos;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
  
        [HttpGet]
        public async Task<ActionResult<ServerResponse<IEnumerable<GetCustomerDto>>>> GetCustomers()
        {
            return HandleResponse(await _customerService.GetAllCustomers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<GetCustomerDto>>> GetCustomer(int id)
        {
            return HandleResponse(await _customerService.GetCustomerById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServerResponse<GetCustomerDto>>> PostCustomer(AddCustomerDto customer)
        {
            return HandleResponse(await _customerService.AddCustomer(customer));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> PutCustomer(int id, UpdateCustomerDto customer)
        {
            return HandleResponse(await _customerService.UpdateCustomer(id, customer));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> DeleteCustomer(int id)
        {
            return HandleResponse(await _customerService.DeleteCustomer(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServerResponse<T>> HandleResponse<T> (ServerResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}