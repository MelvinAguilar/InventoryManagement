using System.Net;

namespace InventoryApp.Client.Services.Impl
{
    public class SupplyService : ISupplyService
    {
        private readonly HttpClient _httpClient;

        public SupplyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<List<GetSupplyDto>>> GetSupplies()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<GetSupplyDto>>>("api/supply");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<List<GetSupplyDto>>("Supplies not found");
                return Response.ErrorResponse<List<GetSupplyDto>>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetSupplyDto>> GetSupply(int id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<GetSupplyDto>>($"api/supply/{id}");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<GetSupplyDto>("Supply not found");
                return Response.ErrorResponse<GetSupplyDto>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetSupplyDto>> AddSupply(AddSupplyDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/supply", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<GetSupplyDto>>());
        }

        public async Task<ServiceResponse<bool>> UpdateSupply(UpdateSupplyDto request)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/supply/{request.Id}", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }
    }
}