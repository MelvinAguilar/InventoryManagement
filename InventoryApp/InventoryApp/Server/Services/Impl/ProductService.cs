using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly inventory_managementContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(inventory_managementContext context, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of products wrapped in a service response</returns>
        public async Task<ServiceResponse<IEnumerable<GetProductDto>>> GetAllProducts()
        {
            var response = new ServiceResponse<IEnumerable<GetProductDto>>();
            // Get all products with his categories related to it
            var products = await _context.Products.Include(p => p.IdCategoryNavigation).ToListAsync();

            if (products == null)
            {
                response.Success = false;
                response.Message = "No products found";
            }
            else
            {
                response.Data = _mapper.Map<IEnumerable<GetProductDto>>(products);
            }

            return response;
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>Product wrapped in a service response</returns>
        public async Task<ServiceResponse<GetProductDto>> GetProductById(int id)
        {
            var response = new ServiceResponse<GetProductDto>();
            // Get product with his categories related to it
            var product = await _context.Products.Include(p => p.IdCategoryNavigation)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found";
            }
            else
            {
                response.Data = _mapper.Map<GetProductDto>(product);
            }

            return response;
        }

        /// <summary>
        /// Add new product into database
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Added product wrapped in a service response</returns>
        public async Task<ServiceResponse<GetProductDto>> AddProduct(AddProductDto product)
        {
            var newProduct = _mapper.Map<Product>(product);
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return new ServiceResponse<GetProductDto> { Data = _mapper.Map<GetProductDto>(newProduct) };
        }

        /// <summary>
        /// Update product in database
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="product">Product to update</param>
        /// <returns>Success or error message in service response</returns>
        public async Task<ServiceResponse<bool>> UpdateProduct(int id, UpdateProductDto product)
        {
            var response = new ServiceResponse<bool>();
            
            if (id != product.Id)
            {
                response.Success = false;
                response.Message = "Product id mismatch";
                return response;
            }
            
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null) 
            {
                response.Success = false;
                response.Message = "Product not found";
            } else {
                // Update only a few properties of the product in database
                // I dont want to update the DateCreated property
                // that's why I don't use: "_context.Entry(product).State = EntityState.Modified;"
                _context.Products.Attach(existingProduct);
                existingProduct.Name = product.Name;
                existingProduct.Specification = product.Specification;
                existingProduct.Brand = product.Brand;
                existingProduct.Stock = product.Stock;
                existingProduct.Price = product.Price;
                existingProduct.Image = product.Image;
                existingProduct.IdCategory = product.IdCategory;
                existingProduct.DateModified = DateTime.Now;

                await _context.SaveChangesAsync();

                response.Data = true;
            }

            return response;
        }

        /// <summary>
        /// Delete product from database
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>Success or error message in service response</returns>
        public async Task<ServiceResponse<bool>> DeleteProduct(int id)
        {
            var response = new ServiceResponse<bool>();
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found";
            }
            else
            {
                // Try catch block for deleting customer products with related orders
                try 
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();

                    response.Data = true;
                }
                catch (DbUpdateException)
                {
                    response.Success = false;
                    response.Message = "Error deleting product: Product has related orders";
                }
            }

            return response;
        }

        /// <summary>
        /// Check if product exists in database
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>True if product exists, false otherwise</returns>
        public bool ProductExists(int id)
            => (_context.Products?.Any(p => p.Id == id)).GetValueOrDefault();

        /// <summary>
        /// Get the ID of the authenticated employee
        /// </summary>
        /// <returns>Employee ID</returns>
        /* This method may be used in the future to auditory the employee who submits request
           to change a product*/
        private int GetAuthenticatedEmployeeId()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context found");
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}