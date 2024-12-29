using Microsoft.Extensions.Options;
using RFService.Entities;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledName<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsIdUuidEnabled<TRepo, TEntity>(repo),
            IServiceName<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuidEnabledName
    {
        public async Task<TEntity> GetSingleForNameAsync(string name, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Name"] = name;

            return await GetSingleAsync(options);
        }

        public async Task<TEntity?> GetSingleOrDefaultForNameAsync(string name, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Name"] = name;

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
