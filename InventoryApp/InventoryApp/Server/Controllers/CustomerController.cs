using InventoryApp.Server.Dtos.CustomerDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
  
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetCustomerDto>>>> GetCustomers()
        {
            return HandleResponse(await _customerService.GetAllCustomers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> GetCustomer(int id)
        {
            return HandleResponse(await _customerService.GetCustomerById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> PostCustomer(AddCustomerDto customer)
        {
            return HandleResponse(await _customerService.AddCustomer(customer));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> PutCustomer(int id, UpdateCustomerDto customer)
        {
            return HandleResponse(await _customerService.UpdateCustomer(id, customer));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteCustomer(int id)
        {
            return HandleResponse(await _customerService.DeleteCustomer(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServiceResponse<T>> HandleResponse<T> (ServiceResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}