using InventoryApp.Server.Dtos.EmployeeDtos;

namespace InventoryApp.Server.Services
{
    public interface IAuthRepository
    {
        Task<ServerResponse<int>> Register(AddEmployeeDto employee, string password);
        Task<ServerResponse<string>> Login(string username, string password);
        Task<ServerResponse<bool>> ForgotPassword(string email);
        Task<ServerResponse<bool>> ResetPassword(ResetPasswordRequest request);
        Task<bool> EmployeeExists(string username, string phoneNumber);
    }
}