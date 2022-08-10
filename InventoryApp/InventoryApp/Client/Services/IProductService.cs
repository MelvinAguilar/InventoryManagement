namespace InventoryApp.Client.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<GetProductDto>>> GetProducts();
        Task<ServiceResponse<GetProductDto>> GetProduct(int id);
        Task<ServiceResponse<GetProductDto>> AddProduct(AddProductDto request);
        Task<ServiceResponse<bool>> UpdateProduct(UpdateProductDto request);
        Task<ServiceResponse<bool>> DeleteProduct(int id); 
    }
}