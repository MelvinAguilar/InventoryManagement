using InventoryApp.Server.Dtos.CategoryDtos;

namespace InventoryApp.Server.Services
{
    public interface ICategoryService
    {
        Task<ServerResponse<IEnumerable<GetCategoryDto>>> GetAllCategories();
        Task<ServerResponse<GetCategoryDto>> GetCategoryById(int id);
        Task<ServerResponse<GetCategoryDto>> AddCategory(AddCategoryDto category);
        Task<ServerResponse<bool>> UpdateCategory(int id, UpdateCategoryDto category);
        Task<ServerResponse<bool>> DeleteCategory(int id);
        
    }
}