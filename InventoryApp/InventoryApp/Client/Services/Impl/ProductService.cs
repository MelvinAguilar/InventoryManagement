using System.Net;

namespace InventoryApp.Client.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetProducts()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<GetProductDto>>>("api/product");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<List<GetProductDto>>("Products not found");
                return Response.ErrorResponse<List<GetProductDto>>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetProductDto>> GetProduct(int id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<GetProductDto>>($"api/product/{id}");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<GetProductDto>("Product not found");
                return Response.ErrorResponse<GetProductDto>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetProductDto>> AddProduct(AddProductDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/product", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<GetProductDto>>());
        }

        public async Task<ServiceResponse<bool>> UpdateProduct(UpdateProductDto request)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/product/{request.Id}", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/product/{id}");
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }
    }
}