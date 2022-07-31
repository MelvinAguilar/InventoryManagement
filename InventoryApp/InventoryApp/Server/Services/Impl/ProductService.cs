using AutoMapper;
using InventoryApp.Server.Dtos.ProductDtos;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class ProductService : IProductService
    {
        public readonly inventory_managementContext _context;
        public readonly IMapper _mapper;

        public ProductService(inventory_managementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of products wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<GetProductDto>>> GetAllProducts()
        {
            var response = new ServerResponse<IEnumerable<GetProductDto>>();
            var products = await _context.Products.ToListAsync();

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
        /// <returns>Product wrapped in a response</returns>
        public async Task<ServerResponse<GetProductDto>> GetProductById(int id)
        {
            var response = new ServerResponse<GetProductDto>();
            var product = await _context.Products.FindAsync(id);

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
        /// <returns>Added product wrapped in a response</returns>
        public async Task<ServerResponse<GetProductDto>> AddProduct(AddProductDto product)
        {
            var newProduct = _mapper.Map<Product>(product);
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return new ServerResponse<GetProductDto> { Data = _mapper.Map<GetProductDto>(newProduct) };
        }

        /// <summary>
        /// Update product in database
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="product">Product to update</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdateProduct(int id, UpdateProductDto product)
        {
            var response = new ServerResponse<bool>();
            
            if (id != product.Id)
            {
                response.Success = false;
                response.Message = "Product id mismatch";
            }
            else
            {
                try 
                {
                    // TODO: Update only a part of the entity
                    var updatedProduct = _mapper.Map<Product>(product);
                    _context.Products.Update(updatedProduct);
                    await _context.SaveChangesAsync();
                    
                    response.Data = true;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    response.Success = false;
                    if (!ProductExists(product.Id))
                        response.Message = "Product not found";
                    else
                        response.Message = "Error updating product: " + e.Message;
                }
            }

            return response;
        }

        /// <summary>
        /// Delete product from database
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> DeleteProduct(int id)
        {
            var response = new ServerResponse<bool>();
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found";
            }
            else
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                
                response.Data = true;
            }

            return response;
        }

        /// <summary>
        /// Check if product exists in database
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>True if product exists, false otherwise</returns>
        public bool ProductExists(int id)
            => (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}