using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class PurchaseService : IPurchaseService
    {
        public readonly inventory_managementContext _context;

        public PurchaseService(inventory_managementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all purchases
        /// </summary>
        /// <returns>List of purchases wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<Purchase>>> GetAllPurchases()
        {
            var response = new ServerResponse<IEnumerable<Purchase>>();
            var purchases = await _context.Purchases.ToListAsync();

            if (purchases == null)
            {
                response.Success = false;
                response.Message = "No purchases found";
            }
            else
            {
                response.Data = purchases;
            }

            return response;
        }

        /// <summary>
        /// Get purchase by id
        /// </summary>
        /// <param name="id">Purchase id</param>
        /// <returns>Purchase wrapped in a response</returns>
        public async Task<ServerResponse<Purchase>> GetPurchaseById(int id)
        {
            var response = new ServerResponse<Purchase>();
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                response.Success = false;
                response.Message = "Purchase not found";
            }
            else
            {
                response.Data = purchase;
            }

            return response;
        }

        /// <summary>
        /// Add new purchase into database
        /// </summary>
        /// <param name="purchase">Purchase to add</param>
        /// <returns>Added purchase wrapped in a response</returns>
        public async Task<ServerResponse<Purchase>> AddPurchase(Purchase purchase)
        {
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return new ServerResponse<Purchase> { Data = purchase };
        }

        /// <summary>
        /// Update purchase in database
        /// </summary>
        /// <param name="id">Purchase Id</param>
        /// <param name="purchase">Purchase to update</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdatePurchase(int id, Purchase purchase)
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
                    _context.Entry(purchase).State = EntityState.Modified;
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
        /// Delete purchase from database
        /// </summary>
        /// <param name="id">Purchase Id</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> DeletePurchase(int id)
        {
            var response = new ServerResponse<bool>();
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                response.Success = false;
                response.Message = "Purchase not found";
            }
            else
            {
                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync();

                response.Data = true;
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