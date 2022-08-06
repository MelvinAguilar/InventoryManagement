namespace InventoryApp.Client.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<string>> Login(EmployeeLoginDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            var serviceResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
                      
            return (serviceResponse != null) ? 
                serviceResponse : 
                new ServiceResponse<string> { Success = false, Message = "Error while logging in" };
        }

        public async Task<ServiceResponse<int>> Register(AddEmployeeDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            var serviceResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
                      
            return (serviceResponse != null) ? 
                serviceResponse : 
                new ServiceResponse<int> { Success = false, Message = "Error while registering" };
        }        

        public async Task<ServiceResponse<bool>> ForgotPassword(ForgotPasswordRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", request.Email);
            var serviceResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return (serviceResponse != null) ? 
                serviceResponse : 
                new ServiceResponse<bool> { Success = false, Message = "Error while sending forgot password email" };
        }

        public async Task<ServiceResponse<bool>> ResetPassword(ResetPasswordRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
            var serviceResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return (serviceResponse != null) ? 
                serviceResponse : 
                new ServiceResponse<bool> { Success = false, Message = "Error while resetting password" };
        }

    }
}