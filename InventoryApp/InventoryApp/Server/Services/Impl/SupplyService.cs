using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class SupplyService : ISupplyService
    {
        public readonly inventory_managementContext _context;

        public SupplyService(inventory_managementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all supplies
        /// </summary>
        /// <returns>List of supplies wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<Supply>>> GetAllSupplies()
        {
            var response = new ServerResponse<IEnumerable<Supply>>();
            var supplies = await _context.Supplies.ToListAsync();

            if (supplies == null)
            {
                response.Success = false;
                response.Message = "No supplies found";
            }
            else
            {
                response.Data = supplies;
            }

            return response;
        }

        /// <summary>
        /// Get supply by id
        /// </summary>
        /// <param name="id">Supply id</param>
        /// <returns>Supply wrapped in a response</returns>
        public async Task<ServerResponse<Supply>> GetSupplyById(int id)
        {
            var response = new ServerResponse<Supply>();
            var supply = await _context.Supplies.FindAsync(id);

            if (supply == null)
            {
                response.Success = false;
                response.Message = "Supply not found";
            }
            else
            {
                response.Data = supply;
            }

            return response;
        }

        /// <summary>
        /// Add new supply into database
        /// </summary>
        /// <param name="supply">Supply to add</param>
        /// <returns>Added supply wrapped in a response</returns>
        public async Task<ServerResponse<Supply>> AddSupply(Supply supply)
        {
            _context.Supplies.Add(supply);
            await _context.SaveChangesAsync();

            return new ServerResponse<Supply> { Data = supply };
        }
        
        /// <summary>
        /// Update supply in database
        /// </summary>
        /// <param name="id">Supply Id</param>
        /// <param name="supply">Supply to update</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdateSupply(int id, Supply supply)
        {
            var response = new ServerResponse<bool>();
            if (id != supply.Id)
            {
                response.Success = false;
                response.Message = "Supply id mismatch";
            }
            else
            {
                try 
                {
                    _context.Update(supply);
                    await _context.SaveChangesAsync();

                    response.Data = true;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    response.Success = false;
                    if (!SupplyExists(supply.Id))
                        response.Message = "Supply not found";
                    else 
                        response.Message = "Error updating supply: " + e.Message;
                }
            }
            return response;
        }

        /// <summary>
        /// Delete supply from database
        /// </summary>
        /// <param name="id">Supply id</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> DeleteSupply(int id)
        {
            var response = new ServerResponse<bool>();
            var supply = await _context.Supplies.FindAsync(id);

            if (supply == null)
            {
                response.Success = false;
                response.Message = "Supply not found";
            }
            else
            {
                _context.Supplies.Remove(supply);
                await _context.SaveChangesAsync();

                response.Data = true;
            }

            return response;
        }

        /// <summary>
        /// Check if supply exists in database
        /// </summary>
        /// <param name="id">Supply id</param>
        /// <returns>True if supply exists, false otherwise</returns>
        private bool SupplyExists(int id) 
            => (_context.Supplies?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}