using RFService.Entities;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceIdUuidName<TRepo, TEntity>(TRepo repo)
        : ServiceIdUuid<TRepo, TEntity>(repo),
            IServiceIdUuid<TEntity>,
            IServiceIdUuidName<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityIdUuidName
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
