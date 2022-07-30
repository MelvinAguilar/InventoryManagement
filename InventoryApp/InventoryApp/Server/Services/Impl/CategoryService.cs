using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly inventory_managementContext _context;

        public CategoryService(inventory_managementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of categories wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<Category>>> GetAllCategories()
        {
            var response = new ServerResponse<IEnumerable<Category>>();
            var categories = await _context.Categories.ToListAsync();

            if (categories == null)
            {
                response.Success = false;
                response.Message = "No categories found";
            }
            else
            {
                response.Data = categories;
            }

            return response;
        }

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>Category wrapped in a response</returns>
        public async Task<ServerResponse<Category>> GetCategoryById(int id)
        {
            var response = new ServerResponse<Category>();
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                response.Success = false;
                response.Message = "Category not found";
            } else
            {
                response.Data = category;
            }

            return response;
        }

        /// <summary>
        /// Add new category into database
        /// </summary>
        /// <param name="category">Category to insert into database</param>
        /// <returns>Added category wrapped in a response</returns>  
        public async Task<ServerResponse<Category>> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            
            return new ServerResponse<Category> { Data = category };
        }

        /// <summary>
        /// Update category in database
        /// </summary>
        /// <param name="id">Category id to update</param>
        /// <param name="category">Category</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdateCategory(int id, Category category)
        {
            var response = new ServerResponse<bool>();
            if (id != category.Id)
            {
                response.Success = false;
                response.Message = "Category id mismatch";
            }
            else
            {
                try
                {
                    _context.Entry(category).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    
                    response.Data = true;
                } 
                catch (DbUpdateConcurrencyException e)
                {
                    response.Success = false;
                    if (!CategoryExists(id))
                        response.Message = "Category not found";
                    else
                        response.Message = "Error updating category: " + e.Message;
                }
            }

            return response;
        }

        /// <summary>
        /// Delete category from database
        /// </summary>
        /// <param name="id">Category id to delete</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> DeleteCategory(int id)
        {
            var response = new ServerResponse<bool>();
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                response.Success = false;
                response.Message = "Category not found";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                
                response.Data = true;
            }
            
            return response;
        }

        /// <summary>
        /// Check if category exists
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>True if category exists, false otherwise</returns>      
        private bool CategoryExists(int id)
            => (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}