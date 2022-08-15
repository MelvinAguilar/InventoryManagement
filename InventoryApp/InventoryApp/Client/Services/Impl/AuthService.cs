using System.Net;

namespace InventoryApp.Client.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> IsAuthenticated()
        {
            var authToken = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return (authToken.User.Identity==null) ? false : authToken.User.Identity.IsAuthenticated;
        }

        public async Task<bool> IsInRoleAdmin(string role)
        {
            var authToken = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var claim = authToken.User.Claims.FirstOrDefault(c => c.Type == "role");
            return (claim==null) ? false : claim.Value == role;
        }

        public async Task<ServiceResponse<string>> Login(EmployeeLoginDto request)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/auth/login", request);
                return Response.HandleResponse(
                    await result.Content.ReadFromJsonAsync<ServiceResponse<string>>());
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<string>("Employee not found");
                return Response.ErrorResponse<string>("An error occurred " + ex.Message);
            }
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
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }

        public async Task<ServiceResponse<bool>> ResetPassword(ResetPasswordRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }

        public async Task<ServiceResponse<bool>> UpdatePassword(UpdatePasswordRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/update-password", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }
    }
}