using System.Net;

namespace InventoryApp.Client.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<List<GetCustomerDto>>> GetCustomers()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<GetCustomerDto>>>("api/customer");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<List<GetCustomerDto>>("Customers not found");
                return Response.ErrorResponse<List<GetCustomerDto>>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetCustomerDto>> GetCustomer(int id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<GetCustomerDto>>($"api/customer/{id}");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<GetCustomerDto>("Customer not found");
                return Response.ErrorResponse<GetCustomerDto>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetCustomerDto>> AddCustomer(AddCustomerDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/customer", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<GetCustomerDto>>());
        }

        public async Task<ServiceResponse<bool>> UpdateCustomer(UpdateCustomerDto request)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/customer/{request.Id}", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }

        public async Task<ServiceResponse<bool>> DeleteCustomer(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/customer/{id}");
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }
    }
}