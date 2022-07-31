using AutoMapper;
using InventoryApp.Server.Dtos.PurchaseDtos;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class PurchaseService : IPurchaseService
    {
        public readonly inventory_managementContext _context;
        public readonly IMapper _mapper;

        public PurchaseService(inventory_managementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all purchases
        /// </summary>
        /// <returns>List of purchases wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<GetPurchaseDto>>> GetAllPurchases()
        {
            var response = new ServerResponse<IEnumerable<GetPurchaseDto>>();
            var purchases = await _context.Purchases.ToListAsync();

            if (purchases == null)
            {
                response.Success = false;
                response.Message = "No purchases found";
            }
            else
            {
                response.Data = _mapper.Map<IEnumerable<GetPurchaseDto>>(purchases);
            }

            return response;
        }

        /// <summary>
        /// Get purchase by id
        /// </summary>
        /// <param name="id">Purchase id</param>
        /// <returns>Purchase wrapped in a response</returns>
        public async Task<ServerResponse<GetPurchaseDto>> GetPurchaseById(int id)
        {
            var response = new ServerResponse<GetPurchaseDto>();
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                response.Success = false;
                response.Message = "Purchase not found";
            }
            else
            {
                response.Data = _mapper.Map<GetPurchaseDto>(purchase);
            }

            return response;
        }

        /// <summary>
        /// Add new purchase into database
        /// </summary>
        /// <param name="purchase">Purchase to add</param>
        /// <returns>Added purchase wrapped in a response</returns>
        public async Task<ServerResponse<GetPurchaseDto>> AddPurchase(AddPurchaseDto purchase)
        {
            var newPurchase = _mapper.Map<Purchase>(purchase);
            _context.Purchases.Add(newPurchase);
            await _context.SaveChangesAsync();

            return new ServerResponse<GetPurchaseDto> { Data = _mapper.Map<GetPurchaseDto>(newPurchase) };
        }

        /// <summary>
        /// Update purchase in database
        /// </summary>
        /// <param name="id">Purchase Id</param>
        /// <param name="purchase">Purchase to update</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdatePurchase(int id, UpdatePurchaseDto purchase)
        {
            var response = new ServerResponse<bool>();
            if (id != purchase.Id)
            {
                response.Success = false;
                response.Message = "Purchase id mismatch"; //"Ids do not match"
            } 
            else 
            {
                try 
                {
                    // TODO: Update only a part of the entity
                    var updatedPurchase = _mapper.Map<Purchase>(purchase);
                    _context.Entry(updatedPurchase).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    
                    response.Data = true;
                } 
                catch (DbUpdateConcurrencyException e) 
                {
                    response.Success = false;

                    if (!PurchaseExists(id))
                        response.Message = "Purchase not found";
                    else 
                        response.Message = "Error updating purchase: " + e.Message;
                }
            }

            return response;
        }

        /// <summary>
        /// Check if purchase exists in database
        /// </summary>
        /// <param name="id">Purchase Id</param>
        /// <returns>True if purchase exists, false otherwise</returns>
        private bool PurchaseExists(int id) 
            => (_context.Purchases?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}