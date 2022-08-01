using AutoMapper;
using InventoryApp.Server.Dtos.SupplyDtos;
using Microsoft.EntityFrameworkCore;

/*
    TODO: Add the following:
    1. Add supply details
    2. Update supply details
*/

namespace InventoryApp.Server.Services.Impl
{
    public class SupplyService : ISupplyService
    {
        public readonly inventory_managementContext _context;
        public readonly IMapper _mapper;

        public SupplyService(inventory_managementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all supplies
        /// </summary>
        /// <returns>List of supplies wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<GetSupplyDto>>> GetAllSupplies()
        {
            var response = new ServerResponse<IEnumerable<GetSupplyDto>>();
            // Get all supplies with related entities
            var supplies = await _context.Supplies
                .Include(s => s.IdEmployeeNavigation)
                .Include(s => s.IdProviderNavigation)
                .ToListAsync();
            
            if (supplies == null)
            {
                response.Success = false;
                response.Message = "No supplies found";
            }
            else
            {
                response.Data = _mapper.Map<IEnumerable<GetSupplyDto>>(supplies);
            }

            return response;
        }

        /// <summary>
        /// Get supply by id
        /// </summary>
        /// <param name="id">Supply id</param>
        /// <returns>Supply wrapped in a response</returns>
        public async Task<ServerResponse<GetSupplyDto>> GetSupplyById(int id)
        {
            var response = new ServerResponse<GetSupplyDto>();
            // Get supply with related entities
            var supply = await _context.Supplies
                .Include(s => s.IdEmployeeNavigation)
                .Include(s => s.IdProviderNavigation)
                .Include(s => s.SupplyDetails)
                .ThenInclude(sd => sd.IdProductNavigation)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supply == null)
            {
                response.Success = false;
                response.Message = "Supply not found";
            }
            else
            {
                response.Data = _mapper.Map<GetSupplyDto>(supply);
            }

            return response;
        }

        /// <summary>
        /// Add new supply into database
        /// </summary>
        /// <param name="supply">Supply to add</param>
        /// <returns>Added supply wrapped in a response</returns>
        public async Task<ServerResponse<GetSupplyDto>> AddSupply(AddSupplyDto supply)
        {
            var newSupply = _mapper.Map<Supply>(supply);
            _context.Supplies.Add(newSupply);
            await _context.SaveChangesAsync();

            return new ServerResponse<GetSupplyDto> { Data = _mapper.Map<GetSupplyDto>(newSupply) };
        }
        
        /// <summary>
        /// Update supply in database
        /// </summary>
        /// <param name="id">Supply Id</param>
        /// <param name="supply">Supply to update</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdateSupply(int id, UpdateSupplyDto supply)
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
                    // TODO: Update only a part of the entity
                    var updatedSupply = _mapper.Map<Supply>(supply);
                    _context.Entry(updatedSupply).State = EntityState.Modified;
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
        /// Check if supply exists in database
        /// </summary>
        /// <param name="id">Supply id</param>
        /// <returns>True if supply exists, false otherwise</returns>
        private bool SupplyExists(int id) 
            => (_context.Supplies?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}