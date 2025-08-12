using RFService.Entities;
using RFService.ILibs;
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
        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceName<TEntity>)this).SanitizeNameForAutoGet(data)
            );

        public async Task<TEntity> GetSingleForNameAsync(string name, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Name", name);

            return await GetSingleAsync(options);
        }

        public async Task<TEntity?> GetSingleOrDefaultForNameAsync(string name, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Name", name);

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
