namespace InventoryApp.Server.Services
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(AddEmployeeDto employee, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<bool>> ForgotPassword(string email);
        Task<ServiceResponse<bool>> ResetPassword(ResetPasswordRequest request);
        Task<ServiceResponse<bool>> UpdatePassword(int employeeId, string oldPassword, string newPassword);
        Task<bool> EmployeeExists(string username, string phoneNumber);
    }
}