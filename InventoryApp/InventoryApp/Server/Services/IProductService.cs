using InventoryApp.Server.Dtos.ProductDtos;

namespace InventoryApp.Server.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<IEnumerable<GetProductDto>>> GetAllProducts();
        Task<ServiceResponse<GetProductDto>> GetProductById(int id);
        Task<ServiceResponse<GetProductDto>> AddProduct(AddProductDto product);
        Task<ServiceResponse<bool>> UpdateProduct(int id, UpdateProductDto product);
        Task<ServiceResponse<bool>> DeleteProduct(int id);
    }
}