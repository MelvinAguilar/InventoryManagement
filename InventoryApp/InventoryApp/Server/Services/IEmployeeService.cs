namespace InventoryApp.Server.Services
{
    public interface IEmployeeService
    {
        Task<ServerResponse<IEnumerable<Employee>>> GetAllEmployees();
        Task<ServerResponse<Employee>> GetEmployeeById(int id);
        Task<ServerResponse<Employee>> AddEmployee(Employee employee);
        Task<ServerResponse<bool>> UpdateEmployee(int id, Employee employee);
        Task<ServerResponse<bool>> DeleteEmployee(int id);
    }
}