using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServerResponse<IEnumerable<Category>>>> GetCategories()
        {
            return HandleResponse(await _categoryService.GetAllCategories());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<Category>>> GetCategory(int id)
        {
            return HandleResponse(await _categoryService.GetCategoryById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServerResponse<Category>>> PostCategory(Category category)
        {
            return HandleResponse(await _categoryService.AddCategory(category));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> PutCategory(int id, Category category)
        {
            return HandleResponse(await _categoryService.UpdateCategory(id, category));
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> DeleteCategory(int id)
        {
            return HandleResponse(await _categoryService.DeleteCategory(id));
        }

        // TODO: Validate the model state ?
        // Method to generalize to avoid code duplication
        private ActionResult<ServerResponse<T>> HandleResponse<T> (ServerResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }    
}