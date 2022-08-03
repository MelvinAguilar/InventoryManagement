using InventoryApp.Server.Dtos.EmployeeDtos;

namespace InventoryApp.Server.Services
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<IEnumerable<GetEmployeeDto>>> GetAllEmployees();
        Task<ServiceResponse<GetEmployeeDto>> GetEmployeeById(int id);
        Task<ServiceResponse<bool>> UpdateEmployee(int id, UpdateEmployeeDto employee);
        Task<ServiceResponse<bool>> DeleteEmployee(int id);
    }
}