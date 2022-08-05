using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
            var response = new ServiceResponse<GetSupplyDto>();
            var newSupply = _mapper.Map<Supply>(supply);

            // Begin a transaction to add new supply and its details
            // For more information, see:  https://www.entityframeworktutorial.net/entityframework6/transaction-in-entity-framework.aspx
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    newSupply.IdEmployee = GetAuthenticatedEmployeeId(); // Get employee id from claims
                    _context.Supplies.Add(newSupply); // Add new supply
                    await _context.SaveChangesAsync(); // Save changes
                    
                    // Commit transaction
                    transaction.Commit();

                    response.Data = _mapper.Map<GetSupplyDto>(newSupply);
                }
                catch (Exception ex)
                {
                    // Rollback transaction if exception occurs
                    transaction.Rollback();
                    response.Success = false;
                    response.Message = "Error while adding supply: " + ex.Message;
                }
            }

            return response;
        }
        
        /// <summary>
        /// Update supply in database
        /// </summary>
        /// <param name="id">Supply Id</param>
        /// <param name="supply">Supply to update</param>
        /// <returns>Success or error message in service response</returns>
        public async Task<ServiceResponse<bool>> UpdateSupply(int id, UpdateSupplyDto request)
        {
            var response = new ServiceResponse<bool>();
            if (id != request.Id)
            {
                response.Success = false;
                response.Message = "Supply id mismatch";
                return response;
            }

            // Get supply with supply details
            var existingSupply = await _context.Supplies
                .Include(s => s.SupplyDetails)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingSupply == null)
            {
                response.Success = false;
                response.Message = "Supply not found";
                return response;
            } 
            else if ( existingSupply.DateSupplied.AddHours(2) < DateTime.Now) 
            {
                response.Success = false;
                response.Message = "Supply cannot be updated after 2 hours";
                return response;
            }

            // Begin a transaction to update supply and its details
            // For more information, see:  https://www.entityframeworktutorial.net/entityframework6/transaction-in-entity-framework.aspx
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try {
                    _context.Supplies.Attach(existingSupply);
                    existingSupply.TotalAmount = request.TotalAmount;
                    existingSupply.Payment = request.Payment;
                    existingSupply.DateModified = DateTime.Now;
                    existingSupply.SupplyDetails = _mapper.Map<List<SupplyDetail>>(request.SupplyDetails);

                    await _context.SaveChangesAsync();

                    // Commit transaction
                    transaction.Commit();
                    
                    response.Success = true;
                } 
                catch (Exception ex)
                {
                    // Rollback transaction if exception occurs
                    transaction.Rollback();
                    response.Success = false;
                    response.Message = "Error while updating supply: " + ex.Message;
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
        private int GetAuthenticatedEmployeeId()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context found");
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}