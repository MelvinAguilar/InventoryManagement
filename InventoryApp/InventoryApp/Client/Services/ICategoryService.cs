namespace InventoryApp.Client.Services
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<GetCategoryDto>>> GetCategories();
        Task<ServiceResponse<GetCategoryDto>> AddCategory(AddCategoryDto request);
        Task<ServiceResponse<GetCategoryDto>> GetCategory(int id);
        Task<ServiceResponse<bool>> UpdateCategory(UpdateCategoryDto request);
        Task<ServiceResponse<bool>> DeleteCategory(int id);
    }
}