using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetEmployeeDto>>>> GetEmployees()
        {
            return HandleResponse(await _employeeService.GetAllEmployees());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetEmployee(int id)
        {
            return HandleResponse(await _employeeService.GetEmployeeById(id));
        }

        [Authorize (Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> PutEmployee(int id, UpdateEmployeeDto employee)
        {
            return HandleResponse(await _employeeService.UpdateEmployee(id, employee));
        }

        [Authorize (Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteEmployee(int id)
        {
            return HandleResponse(await _employeeService.DeleteEmployee(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServiceResponse<T>> HandleResponse<T> (ServiceResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}