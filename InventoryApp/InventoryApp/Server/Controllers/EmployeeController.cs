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
        public async Task<ActionResult<ServerResponse<IEnumerable<Employee>>>> GetEmployees()
        {
            return HandleResponse(await _employeeService.GetAllEmployees());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<Employee>>> GetEmployee(int id)
        {
            return HandleResponse(await _employeeService.GetEmployeeById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServerResponse<Employee>>> PostEmployee(Employee employee)
        {
            return HandleResponse(await _employeeService.AddEmployee(employee));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> PutEmployee(int id, Employee employee)
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