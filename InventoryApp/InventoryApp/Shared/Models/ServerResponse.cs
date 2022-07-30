namespace InventoryApp.Shared.Models
{
    /// <summary>
    /// Server response for http request; add aditional information to the returning result
    /// </summary>
    
    public class ServerResponse<T>
    {
        /// <summary>
        /// Boolean value indicating if the request was successful or not
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// Message of the request
        /// </summary>
        public string Message { get; set; } = string.Empty;
        /// <summary>
        /// Result of the request
        /// </summary>
        public T? Data { get; set; }

    }
}