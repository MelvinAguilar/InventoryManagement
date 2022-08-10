namespace InventoryApp.Client.Services.Impl
{
    public static class Response
    {
        public static ServiceResponse<T> HandleResponse<T>(ServiceResponse<T>? response)
        {
            if (response == null)
                return ErrorResponse<T>();

            return response;
        }
        
        public static ServiceResponse<T> ErrorResponse<T>(string message = "")
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Data = default(T),
                Message = "Error while processing request " + message
            };
        }
    }
}