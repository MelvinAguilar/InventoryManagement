using InventoryApp.Server.Dtos.EmployeeDtos;

namespace InventoryApp.Server.Services
{
    public interface IEmployeeService
    {
        Task<ServerResponse<IEnumerable<GetEmployeeDto>>> GetAllEmployees();
        Task<ServerResponse<GetEmployeeDto>> GetEmployeeById(int id);
        Task<ServerResponse<GetEmployeeDto>> AddEmployee(AddEmployeeDto employee);
        Task<ServerResponse<bool>> UpdateEmployee(int id, UpdateEmployeeDto employee);
        Task<ServerResponse<bool>> DeleteEmployee(int id);
    }
}