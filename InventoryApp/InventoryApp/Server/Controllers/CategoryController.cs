using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetCategoryDto>>>> GetCategories()
        {
            return HandleResponse(await _categoryService.GetAllCategories());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> GetCategory(int id)
        {
            return HandleResponse(await _categoryService.GetCategoryById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> PostCategory(AddCategoryDto category)
        {
            return HandleResponse(await _categoryService.AddCategory(category));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> PutCategory(int id, UpdateCategoryDto category)
        {
            return HandleResponse(await _categoryService.UpdateCategory(id, category));
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteCategory(int id)
        {
            return HandleResponse(await _categoryService.DeleteCategory(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServiceResponse<T>> HandleResponse<T> (ServiceResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }    
}