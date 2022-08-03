using InventoryApp.Server.Dtos.CategoryDtos;

namespace InventoryApp.Server.Services
{
    public interface ICategoryService
    {
        Task<ServiceResponse<IEnumerable<GetCategoryDto>>> GetAllCategories();
        Task<ServiceResponse<GetCategoryDto>> GetCategoryById(int id);
        Task<ServiceResponse<GetCategoryDto>> AddCategory(AddCategoryDto category);
        Task<ServiceResponse<bool>> UpdateCategory(int id, UpdateCategoryDto category);
        Task<ServiceResponse<bool>> DeleteCategory(int id);
        
    }
}