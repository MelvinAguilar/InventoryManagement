using System.Net;

namespace InventoryApp.Client.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<ServiceResponse<List<GetCategoryDto>>> GetCategories()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<GetCategoryDto>>>("api/category");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<List<GetCategoryDto>>("Categories not found");
                return Response.ErrorResponse<List<GetCategoryDto>>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetCategoryDto>> GetCategory(int id)
        {
            try {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<GetCategoryDto>>($"api/category/{id}");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<GetCategoryDto>("Category not found");
                return Response.ErrorResponse<GetCategoryDto>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetCategoryDto>> AddCategory(AddCategoryDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/category", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<GetCategoryDto>>());
        }

        public async Task<ServiceResponse<bool>> UpdateCategory(UpdateCategoryDto request)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/category/{request.Id}", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }

        public async Task<ServiceResponse<bool>> DeleteCategory(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/category/{id}");
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }
    }
}