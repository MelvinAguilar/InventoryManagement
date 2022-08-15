namespace InventoryApp.Client.Services
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<List<GetEmployeeDto>>> GetEmployees();
        Task<ServiceResponse<GetEmployeeDto>> GetEmployee(int id);
        Task<ServiceResponse<GetEmployeeDto>> GetMe();
        Task<ServiceResponse<GetEmployeeDto>> AddEmployee(AddEmployeeDto request);
        Task<ServiceResponse<bool>> UpdateEmployee(UpdateEmployeeDto request);
        Task<ServiceResponse<bool>> DeleteEmployee(int id);
    }
}