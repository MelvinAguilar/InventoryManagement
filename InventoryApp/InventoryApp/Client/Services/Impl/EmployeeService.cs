using System.Net;

namespace InventoryApp.Client.Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<List<GetEmployeeDto>>> GetEmployees()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<GetEmployeeDto>>>("api/employee");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<List<GetEmployeeDto>>("Employees not found");
                return Response.ErrorResponse<List<GetEmployeeDto>>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetEmployeeDto>> GetEmployee(int id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<GetEmployeeDto>>($"api/employee/{id}");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<GetEmployeeDto>("Employee not found");
                return Response.ErrorResponse<GetEmployeeDto>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetEmployeeDto>> GetMe()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<GetEmployeeDto>>("api/employee/profile");
                return Response.HandleResponse(result);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return Response.ErrorResponse<GetEmployeeDto>("Employee not found");
                return Response.ErrorResponse<GetEmployeeDto>("An error occurred " + ex.Message);
            }
        }

        public async Task<ServiceResponse<GetEmployeeDto>> AddEmployee(AddEmployeeDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/employee", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<GetEmployeeDto>>());
        }

        public async Task<ServiceResponse<bool>> UpdateEmployee(UpdateEmployeeDto request)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/employee/{request.Id}", request);
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }

        public async Task<ServiceResponse<bool>> DeleteEmployee(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/employee/{id}");
            return Response.HandleResponse(
                await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());
        }
    }
}