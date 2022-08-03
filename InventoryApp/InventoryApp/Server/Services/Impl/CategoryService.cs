using System.Security.Claims;
using AutoMapper;
using InventoryApp.Server.Dtos.CategoryDtos;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly inventory_managementContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(inventory_managementContext context, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of categories wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<GetCategoryDto>>> GetAllCategories()
        {
            var response = new ServerResponse<IEnumerable<GetCategoryDto>>();
            var categories = await _context.Categories.ToListAsync();

            if (categories == null)
            {
                response.Success = false;
                response.Message = "No categories found";
            }
            else
            {
                response.Data = _mapper.Map<IEnumerable<GetCategoryDto>>(categories);
            }

            return response;
        }

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>Category wrapped in a response</returns>
        public async Task<ServerResponse<GetCategoryDto>> GetCategoryById(int id)
        {
            var response = new ServerResponse<GetCategoryDto>();
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                response.Success = false;
                response.Message = "Category not found";
            } else
            {
                response.Data = _mapper.Map<GetCategoryDto>(category);
            }

            return response;
        }

        /// <summary>
        /// Add new category into database
        /// </summary>
        /// <param name="category">Category to insert into database</param>
        /// <returns>Added category wrapped in a response</returns>  
        public async Task<ServerResponse<GetCategoryDto>> AddCategory(AddCategoryDto category)
        {
            var response = new ServerResponse<GetCategoryDto>();
            // Try catch block to catch any errors that may occur while inserting into database
            try {
                var newCategory = _mapper.Map<Category>(category);
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCategoryDto>(newCategory);
            } 
            catch (DbUpdateException e)
            {
                // If enter here, it means that the category name is already taken by another category  
                response.Success = false;
                if (CategoryExists(category.Name))
                    response.Message = "Category which name is " + category.Name + " already exists";
                else
                    response.Message = "Error adding category: " + e.Message;
            }
            
            return response;
        }

        /// <summary>
        /// Update category in database
        /// </summary>
        /// <param name="id">Category id to update</param>
        /// <param name="category">Category</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdateCategory(int id, UpdateCategoryDto category)
        {
            var response = new ServerResponse<bool>();
            if (id != category.Id)
            {
                response.Success = false;
                response.Message = "Category id mismatch";
                return response;
            }

            // Get category from database
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                response.Success = false;
                response.Message = "Category not found";
            }
            else
            {
                try
                {
                    // Update only a few properties of the category in database
                    // I dont want to update the DateCreated property
                    // that's why I don't use: "_context.Entry(entity).State = EntityState.Modified;"
                    _context.Categories.Attach(existingCategory);
                    existingCategory.Name = category.Name;
                    existingCategory.Description = category.Description;
                    existingCategory.DateModified = DateTime.Now;

                    await _context.SaveChangesAsync();
                    response.Data = true;
                }
                catch (DbUpdateException e)
                {
                    // If enter here, it means that the category name is already taken by another category
                    response.Success = false;
                    if (CategoryExists(category.Name))
                        response.Message = "Category which name is " + category.Name + " already exists";
                    else
                        response.Message = "Error updating category: " + e.Message;
                }
            }

            return response;
        }

        /// <summary>
        /// Delete category from database
        /// </summary>s
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
                // Try catch block for deleting category with foreign key constraint error
                try
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                    
                    response.Data = true;
                } 
                catch (DbUpdateException)
                {
                    response.Success = false;
                    response.Message = "Error deleting category: There are products that contain this category";
                }
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

        /// <summary>
        /// Check if category exists
        /// </summary>
        /// <param name="name">Category name</param>
        /// <returns>True if category exists, false otherwise</returns>
        private bool CategoryExists(string name)
            => (_context.Categories?.Any(c => c.Name == name)).GetValueOrDefault();

        /// <summary>
        /// Get the ID of the authenticated employee
        /// </summary>
        /// <returns>Employee ID</returns>
        /* This method may be used in the future to auditory the employee who submits request
           to change a category*/
        private int GetAuthenticatedEmployeeId()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context found");
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}