using System.Net;

namespace InventoryApp.Client.Services.Impl
{
    public class ProviderService : IProviderService
    {
        private readonly HttpClient _httpClient;

        public ProviderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<List<GetProviderDto>>> GetProviders()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<GetProviderDto>>>("api/provider");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<List<GetProviderDto>>("Providers not found");
                return Response.ErrorResponse<List<GetProviderDto>>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetProviderDto>> GetProvider(int id)
        {
            try {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<GetProviderDto>>($"api/provider/{id}");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<GetProviderDto>("Provider not found");
                return Response.ErrorResponse<GetProviderDto>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetProviderDto>> AddProvider(AddProviderDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/provider", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<GetProviderDto>>());
        }

        public async Task<ServiceResponse<bool>> UpdateProvider(UpdateProviderDto request)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/provider/{request.Id}", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }

        public async Task<ServiceResponse<bool>> DeleteProvider(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/provider/{id}");
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }

        
    }
}