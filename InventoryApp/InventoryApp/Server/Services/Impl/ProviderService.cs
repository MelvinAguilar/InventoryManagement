using System.Security.Claims;
using AutoMapper;
using InventoryApp.Server.Dtos.ProviderDtos;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class ProviderService : IProviderService
    {
        private readonly inventory_managementContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProviderService(inventory_managementContext context, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all providers
        /// </summary>
        /// <returns>List of providers wrapped in a service response</returns>
        public async Task<ServiceResponse<IEnumerable<GetProviderDto>>> GetAllProviders()
        {
            var response = new ServiceResponse<IEnumerable<GetProviderDto>>();
            var providers = await _context.Providers.ToListAsync();

            if (providers == null)
            {
                response.Success = false;
                response.Message = "No providers found";
            }
            else
            {
                response.Data = _mapper.Map<IEnumerable<GetProviderDto>>(providers);
            }

            return response;
        }

        /// <summary>
        /// Get provider by id
        /// </summary>
        /// <param name="id">Provider id</param>
        /// <returns>Provider wrapped in a service response</returns>
        public async Task<ServiceResponse<GetProviderDto>> GetProviderById(int id)
        {
            var response = new ServiceResponse<GetProviderDto>();
            var provider = await _context.Providers.FindAsync(id);

            if (provider == null)
            {
                response.Success = false;
                response.Message = "Provider not found";
            }
            else
            {
                response.Data = _mapper.Map<GetProviderDto>(provider);
            }

            return response;
        }

        /// <summary>
        /// Add new provider into database
        /// </summary>
        /// <param name="provider">Provider to add</param>
        /// <returns>Added provider wrapped in a service response</returns>
        public async Task<ServiceResponse<GetProviderDto>> AddProvider(AddProviderDto provider)
        {
            var response = new ServiceResponse<GetProviderDto>();
            // Try catch block to catch any errors that may occur while inserting into database
            try
            {
                var newProvider = _mapper.Map<Provider>(provider);
                _context.Providers.Add(newProvider);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetProviderDto>(newProvider);
            }
            catch (DbUpdateException ex)
            {
                // if enters here, it means that the provider already exists in database
                response.Success = false;
                if (ProviderExists(provider.Name, provider.PhoneNumber))
                    response.Message = "Provider with name " + provider.Name + " or phone number " + provider.PhoneNumber + " already exists";
                else
                    response.Message = "Error occured while adding provider: " + ex.Message;;
            }            

            return response;
        }

        /// <summary>
        /// Update provider in database
        /// </summary>
        /// <param name="id">Provider Id</param>
        /// <param name="provider">Provider to update</param>
        /// <returns>Success or failure message in a service response</returns>
        public async Task<ServiceResponse<bool>> UpdateProvider(int id, UpdateProviderDto provider)
        {
            var response = new ServiceResponse<bool>();
            if (id != provider.Id)
            {
                response.Success = false;
                response.Message = "Provider id mismatch";
                return response;
            }

            var existingProvider = await _context.Providers.FindAsync(id);
            if (existingProvider == null)
            {
                response.Success = false;
                response.Message = "Provider not found";
                return response;
            }
            else
            {
                try
                {
                    // Update only a few properties of the provider in database
                    // I dont want to update the DateCreated property
                    // that's why I don't use: "_context.Entry(entity).State = EntityState.Modified;"
                    _context.Providers.Attach(existingProvider);
                    existingProvider.Name = provider.Name;
                    existingProvider.Address = provider.Address;
                    existingProvider.PhoneNumber = provider.PhoneNumber;
                    existingProvider.Description = provider.Description;
                    existingProvider.DateModified = DateTime.Now;
                    
                    await _context.SaveChangesAsync();
                    response.Data = true;
                }
                catch (DbUpdateException ex)
                {
                    // if enters here, it means that the provider already exists in database
                    response.Success = false;
                    if (ProviderExists(provider.Name, provider.PhoneNumber))
                        response.Message = "Provider with name " + provider.Name + " or phone number " + provider.PhoneNumber + " already exists";
                    else
                        response.Message = "Error occured while updating provider: " + ex.Message;
                }
            }

            return response;
        }

        /// <summary>
        /// Delete provider from database
        /// </summary>
        /// <param name="id">Provider id to delete</param>
        /// <returns>Success or error message in service response</returns>
        public async Task<ServiceResponse<bool>> DeleteProvider(int id)
        {
            var response = new ServiceResponse<bool>();
            var provider = await _context.Providers.FindAsync(id);

            if (provider == null)
            {
                response.Success = false;
                response.Message = "Provider not found";
            }
            else
            {
                // Try catch block to catch any errors that may occur while deleting from database
                try
                {
                    _context.Providers.Remove(provider);
                    await _context.SaveChangesAsync();

                    response.Success = true;
                }
                catch (DbUpdateException)
                {
                    response.Success = false;
                    response.Message = "Error occured while deleting provider: This provider is being used by other entities";  
                }
            }

            return response;
        }
        
        /// <summary>
        /// Check if provider exists in database
        /// </summary>
        /// <param name="id">Provider id</param>
        /// <returns>True if provider exists, false otherwise</returns>
        private bool ProviderExists(int id) 
            => (_context.Providers?.Any(e => e.Id == id)).GetValueOrDefault();

        /// <summary>
        /// Check if provider exists in database
        /// </summary>
        /// <param name="name">Provider name</param>
        /// <param name="phoneNumber">Provider phone number</param>
        /// <returns>True if provider exists, false otherwise</returns>
        private bool ProviderExists(string name, string phoneNumber)
            => (_context.Providers?.Any(e => e.Name == name || e.PhoneNumber == phoneNumber)).GetValueOrDefault();

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