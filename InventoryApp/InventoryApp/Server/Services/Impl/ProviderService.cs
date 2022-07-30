using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class ProviderService : IProviderService
    {
        public readonly inventory_managementContext _context;

        public ProviderService(inventory_managementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all providers
        /// </summary>
        /// <returns>List of providers wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<Provider>>> GetAllProviders()
        {
            var response = new ServerResponse<IEnumerable<Provider>>();
            var providers = await _context.Providers.ToListAsync();

            if (providers == null)
            {
                response.Success = false;
                response.Message = "No providers found";
            }
            else
            {
                response.Data = providers;
            }

            return response;
        }

        /// <summary>
        /// Get provider by id
        /// </summary>
        /// <param name="id">Provider id</param>
        /// <returns>Provider wrapped in a response</returns>
        public async Task<ServerResponse<Provider>> GetProviderById(int id)
        {
            var response = new ServerResponse<Provider>();
            var provider = await _context.Providers.FindAsync(id);

            if (provider == null)
            {
                response.Success = false;
                response.Message = "Provider not found";
            }
            else
            {
                response.Data = provider;
            }

            return response;
        }

        /// <summary>
        /// Add new provider into database
        /// </summary>
        /// <param name="provider">Provider to add</param>
        /// <returns>Added provider wrapped in a response</returns>
        public async Task<ServerResponse<Provider>> AddProvider(Provider provider)
        {
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            return new ServerResponse<Provider> { Data = provider };
        }

        /// <summary>
        /// Update provider in database
        /// </summary>
        /// <param name="id">Provider Id</param>
        /// <param name="provider">Provider to update</param>
        /// <returns>Server response</returns>
        public async Task<ServerResponse<bool>> UpdateProvider(int id, Provider provider)
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
                    _context.Entry(provider).State = EntityState.Modified;
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