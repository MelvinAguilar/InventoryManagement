using InventoryApp.Server.Dtos.ProductDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetProductDto>>>> GetProducts()
        {
            return HandleResponse(await _productService.GetAllProducts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> GetProduct(int id)
        {
            return HandleResponse(await _productService.GetProductById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> PostProduct(AddProductDto product)
        {
            return HandleResponse(await _productService.AddProduct(product));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> PutProduct(int id, UpdateProductDto product)
        {
            return HandleResponse(await _productService.UpdateProduct(id, product));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
        {
            return HandleResponse(await _productService.DeleteProduct(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServiceResponse<T>> HandleResponse<T> (ServiceResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}