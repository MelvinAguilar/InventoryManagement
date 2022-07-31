using InventoryApp.Server.Dtos.ProductDtos;

namespace InventoryApp.Server.Services
{
    public interface IProductService
    {
        Task<ServerResponse<IEnumerable<GetProductDto>>> GetAllProducts();
        Task<ServerResponse<GetProductDto>> GetProductById(int id);
        Task<ServerResponse<GetProductDto>> AddProduct(AddProductDto product);
        Task<ServerResponse<bool>> UpdateProduct(int id, UpdateProductDto product);
        Task<ServerResponse<bool>> DeleteProduct(int id);
    }
}