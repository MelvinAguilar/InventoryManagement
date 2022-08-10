using System.Net;

namespace InventoryApp.Client.Services.Impl
{
    public class PurchaseService : IPurchaseService
    {
        private readonly HttpClient _httpClient;

        public PurchaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<List<GetPurchaseDto>>> GetPurchases()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<GetPurchaseDto>>>("api/purchase");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<List<GetPurchaseDto>>("Purchases not found");
                return Response.ErrorResponse<List<GetPurchaseDto>>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetPurchaseDto>> GetPurchase(int id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<GetPurchaseDto>>($"api/purchase/{id}");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<GetPurchaseDto>("Purchase not found");
                return Response.ErrorResponse<GetPurchaseDto>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetPurchaseDto>> AddPurchase(AddPurchaseDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/purchase", request);
            return Response.HandleResponse(await result.Content.ReadFromJsonAsync<ServiceResponse<GetPurchaseDto>>());
        }

        public async Task<ServiceResponse<bool>> UpdatePurchase(UpdatePurchaseDto request)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/purchase/{request.Id}", request);
            return Response.HandleResponse(await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }
    }
}