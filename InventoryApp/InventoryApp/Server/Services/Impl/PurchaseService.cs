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
        /// <returns>List of purchases wrapped in a service response</returns>
        public async Task<ServiceResponse<IEnumerable<GetPurchaseDto>>> GetAllPurchases()
        {
            var response = new ServiceResponse<IEnumerable<GetPurchaseDto>>();
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
        /// <returns>Purchase wrapped in a service response</returns>
        public async Task<ServiceResponse<GetPurchaseDto>> GetPurchaseById(int id)
        {
            var response = new ServiceResponse<GetPurchaseDto>();
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
        /// <returns>Added purchase wrapped in a service response</returns>
        public async Task<ServiceResponse<GetPurchaseDto>> AddPurchase(AddPurchaseDto purchase)
        {
            var response = new ServiceResponse<GetPurchaseDto>();
            var newPurchase = _mapper.Map<Purchase>(purchase);

            // Begin a transaction to add new purchase and its details
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    newPurchase.IdEmployee = GetAuthenticatedEmployeeId();
                    _context.Purchases.Add(newPurchase); // Add new purchase
                    await _context.SaveChangesAsync(); // Save changes 

                    // Commit transaction
                    transaction.Commit();

                    response.Data = _mapper.Map<GetPurchaseDto>(newPurchase);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Success = false;
                    response.Message = "Error while adding purchase: " + ex.Message;
                }
            }

            return response;
        }

        /// <summary>
        /// Update purchase in database
        /// </summary>
        /// <param name="id">Purchase Id</param>
        /// <param name="purchase">Purchase to update</param>
        /// <returns>Success or error message in service response</returns>
        public async Task<ServiceResponse<bool>> UpdatePurchase(int id, UpdatePurchaseDto purchase)
        {
            var response = new ServiceResponse<bool>();
            if (id != purchase.Id)
            {
                response.Success = false;
                response.Message = "Purchase id mismatch";
                return response;
            }

            // Get purchase with purchase details
            var existingPurchase = await _context.Purchases
                .Include(p => p.PurchaseDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPurchase == null)
            {
                response.Success = false;
                response.Message = "Purchase not found";
                return response;
            }
            else if (existingPurchase.DatePurchased.AddMinutes(45) < DateTime.Now)
            {
                response.Success = false;
                response.Message = "Purchase cannot be updated after 45 minutes";
                return response;
            }

            // Begin a transaction to update purchase and its details
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Entry(existingPurchase).State = EntityState.Modified;
                    existingPurchase.GrossAmount = purchase.GrossAmount;
                    existingPurchase.Tax = purchase.Tax;
                    existingPurchase.Discount = purchase.Discount;
                    existingPurchase.NetAmount = purchase.NetAmount;
                    existingPurchase.Payment = purchase.Payment;
                    existingPurchase.IdCustomer = purchase.IdCustomer;
                    existingPurchase.DateModified = DateTime.Now;
                    existingPurchase.PurchaseDetails = _mapper.Map<List<PurchaseDetail>>(purchase.PurchaseDetails);
                    
                    await _context.SaveChangesAsync(); // Save changes 
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Success = false;
                    response.Message = "Error while updating purchase: " + ex.Message;
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
        private int GetAuthenticatedEmployeeId()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context found");
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}