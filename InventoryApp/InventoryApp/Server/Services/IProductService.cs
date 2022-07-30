namespace InventoryApp.Server.Services
{
    public interface IProductService
    {
        Task<ServerResponse<IEnumerable<Product>>> GetAllProducts();
        Task<ServerResponse<Product>> GetProductById(int id);
        Task<ServerResponse<Product>> AddProduct(Product product);
        Task<ServerResponse<bool>> UpdateProduct(int id, Product product);
        Task<ServerResponse<bool>> DeleteProduct(int id);
    }
}