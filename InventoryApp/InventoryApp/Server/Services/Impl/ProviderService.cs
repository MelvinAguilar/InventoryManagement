using AutoMapper;
using InventoryApp.Server.Dtos.ProviderDtos;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class ProviderService : IProviderService
    {
        public readonly inventory_managementContext _context;
        public readonly IMapper _mapper;

        public ProviderService(inventory_managementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all providers
        /// </summary>
        /// <returns>List of providers wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<GetProviderDto>>> GetAllProviders()
        {
            var response = new ServerResponse<IEnumerable<GetProviderDto>>();
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
        /// <returns>Provider wrapped in a response</returns>
        public async Task<ServerResponse<GetProviderDto>> GetProviderById(int id)
        {
            var response = new ServerResponse<GetProviderDto>();
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
        /// <returns>Added provider wrapped in a response</returns>
        public async Task<ServerResponse<GetProviderDto>> AddProvider(AddProviderDto provider)
        {
            var newProvider = _mapper.Map<Provider>(provider);
            _context.Providers.Add(newProvider);
            await _context.SaveChangesAsync();

            return new ServerResponse<GetProviderDto> { Data = _mapper.Map<GetProviderDto>(newProvider) };
        }

        /// <summary>
        /// Update provider in database
        /// </summary>
        /// <param name="id">Provider Id</param>
        /// <param name="provider">Provider to update</param>
        /// <returns>Server response</returns>
        public async Task<ServerResponse<bool>> UpdateProvider(int id, UpdateProviderDto provider)
        {
            var response = new ServerResponse<bool>();
            if (id != provider.Id)
            {
                response.Success = false;
                response.Message = "Providerid mismatch";
            }
            else
            {
                try
                {
                    // TODO: Update only a part of the entity
                    var updatedProvider = _mapper.Map<Provider>(provider);
                    _context.Entry(updatedProvider).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    response.Data = true;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    response.Success = false;
                    if (!ProviderExists(id))
                        response.Message = "Provider not found";
                    else
                        response.Message = "Error updating provider: " + e.Message;
                }
            }

            return response;
        }

        /// <summary>
        /// Delete provider from database
        /// </summary>
        /// <param name="id">Provider id to delete</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> DeleteProvider(int id)
        {
            var response = new ServerResponse<bool>();
            var provider = await _context.Providers.FindAsync(id);

            if (provider == null)
            {
                response.Success = false;
                response.Message = "Provider not found";
            }
            else
            {
                _context.Providers.Remove(provider);
                await _context.SaveChangesAsync();
                
                response.Data = true;
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
    }
}