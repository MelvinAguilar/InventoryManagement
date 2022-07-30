namespace InventoryApp.Server.Services
{
    public interface ICategoryService
    {
        Task<ServerResponse<IEnumerable<Category>>> GetAllCategories();
        Task<ServerResponse<Category>> GetCategoryById(int id);
        Task<ServerResponse<Category>> AddCategory(Category category);
        Task<ServerResponse<bool>> UpdateCategory(int id, Category category);
        Task<ServerResponse<bool>> DeleteCategory(int id);
        
    }
}