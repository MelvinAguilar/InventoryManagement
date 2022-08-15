namespace InventoryApp.Client.Services
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticated();
        Task<bool> IsInRoleAdmin(string role);
        Task<ServiceResponse<int>> Register(AddEmployeeDto request);
        Task<ServiceResponse<string>> Login(EmployeeLoginDto request);
        Task<ServiceResponse<bool>> ForgotPassword(ForgotPasswordRequest request);
        Task<ServiceResponse<bool>> ResetPassword(ResetPasswordRequest request);
        Task<ServiceResponse<bool>> UpdatePassword(UpdatePasswordRequest request);
    }
}