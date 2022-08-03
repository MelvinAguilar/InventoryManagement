using System.Security.Claims;
using AutoMapper;
using InventoryApp.Server.Dtos.PurchaseDtos;
using Microsoft.EntityFrameworkCore;

/*
    TODO: Add the following:
    1. Add purchase details 
    2. Update purchase details
*/

namespace InventoryApp.Server.Services.Impl
{
    public class PurchaseService : IPurchaseService
    {
        private readonly inventory_managementContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PurchaseService(inventory_managementContext context, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all purchases
        /// </summary>
        /// <returns>List of purchases wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<GetPurchaseDto>>> GetAllPurchases()
        {
            var response = new ServerResponse<IEnumerable<GetPurchaseDto>>();
            // Get purchases with related entities
            var purchases = await _context.Purchases
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdEmployeeNavigation)
                .ToListAsync();

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
            // Get purchase with related entities and details
            var purchase = await _context.Purchases
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdEmployeeNavigation)
                .Include(p => p.PurchaseDetails)
                .ThenInclude(pd => pd.IdProductNavigation)
                .FirstOrDefaultAsync(p => p.Id == id);

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
                response.Message = "Purchase id mismatch";
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

        /// <summary>
        /// Get the ID of the authenticated employee
        /// </summary>
        /// <returns>Employee ID</returns>
        /* This method may be used in the future to auditory the employee who submits request
           to change a purchase*/
        private int GetAuthenticatedEmployeeId()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context found");
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}