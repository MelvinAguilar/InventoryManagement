using System.Security.Claims;
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
        private readonly inventory_managementContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupplyService(inventory_managementContext context, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all supplies
        /// </summary>
        /// <returns>List of supplies wrapped in a service response</returns>
        public async Task<ServiceResponse<IEnumerable<GetSupplyDto>>> GetAllSupplies()
        {
            var response = new ServiceResponse<IEnumerable<GetSupplyDto>>();
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
        /// <returns>Supply wrapped in a service response</returns>
        public async Task<ServiceResponse<GetSupplyDto>> GetSupplyById(int id)
        {
            var response = new ServiceResponse<GetSupplyDto>();
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
        /// <returns>Added supply wrapped in a service response</returns>
        public async Task<ServiceResponse<GetSupplyDto>> AddSupply(AddSupplyDto supply)
        {
            var newSupply = _mapper.Map<Supply>(supply);
            _context.Supplies.Add(newSupply);
            await _context.SaveChangesAsync();

            return new ServiceResponse<GetSupplyDto> { Data = _mapper.Map<GetSupplyDto>(newSupply) };
        }
        
        /// <summary>
        /// Update supply in database
        /// </summary>
        /// <param name="id">Supply Id</param>
        /// <param name="supply">Supply to update</param>
        /// <returns>Success or error message in service response</returns>
        public async Task<ServiceResponse<bool>> UpdateSupply(int id, UpdateSupplyDto supply)
        {
            var response = new ServiceResponse<bool>();
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

        /// <summary>
        /// Get the ID of the authenticated employee
        /// </summary>
        /// <returns>Employee ID</returns>
        /* This method may be used in the future to auditory the employee who submits request
           to change a provider*/
        private int GetAuthenticatedEmployeeId()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context found");
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }    
    }
}