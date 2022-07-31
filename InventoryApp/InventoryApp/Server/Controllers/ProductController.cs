using InventoryApp.Server.Dtos.ProductDtos;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServerResponse<IEnumerable<GetProductDto>>>> GetProducts()
        {
            return HandleResponse(await _productService.GetAllProducts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<GetProductDto>>> GetProduct(int id)
        {
            return HandleResponse(await _productService.GetProductById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServerResponse<GetProductDto>>> PostProduct(AddProductDto product)
        {
            return HandleResponse(await _productService.AddProduct(product));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> PutProduct(int id, UpdateProductDto product)
        {
            return HandleResponse(await _productService.UpdateProduct(id, product));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServerResponse<bool>>> DeleteProduct(int id)
        {
            return HandleResponse(await _productService.DeleteProduct(id));
        }

        // Method to generalize to avoid code duplication
        private ActionResult<ServerResponse<T>> HandleResponse<T> (ServerResponse<T> response)
        {
            return (response.Success) ? Ok(response) : NotFound(response); 
        }
    }
}