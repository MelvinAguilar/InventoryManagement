namespace InventoryApp.Client.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(AddEmployeeDto request);
        Task<ServiceResponse<string>> Login(EmployeeLoginDto request);
        Task<ServiceResponse<bool>> ForgotPassword(ForgotPasswordRequest request);
        Task<ServiceResponse<bool>> ResetPassword(ResetPasswordRequest request);
    }
}