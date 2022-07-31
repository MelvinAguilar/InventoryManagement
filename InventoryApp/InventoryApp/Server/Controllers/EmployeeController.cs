using InventoryApp.Server.Dtos.EmployeeDtos;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<ServerResponse<IEnumerable<GetEmployeeDto>>>> GetEmployees()
        {
            return HandleResponse(await _employeeService.GetAllEmployees());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<GetEmployeeDto>>> GetEmployee(int id)
        {
            return HandleResponse(await _employeeService.GetEmployeeById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServerResponse<GetEmployeeDto>>> PostEmployee(AddEmployeeDto employee)
        {
            return HandleResponse(await _employeeService.AddEmployee(employee));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> PutEmployee(int id, UpdateEmployeeDto employee)
        {
            return HandleResponse(await _employeeService.UpdateEmployee(id, employee));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> DeleteEmployee(int id)
        {
            return HandleResponse(await _employeeService.DeleteEmployee(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServerResponse<T>> HandleResponse<T> (ServerResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}