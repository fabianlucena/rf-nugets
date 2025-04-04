using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidName<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestampsIdUuid<TRepo, TEntity>(repo),
            IServiceName<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsIdUuidName
    {
        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceName<TEntity>)this).SanitizeNameForAutoGet(data)
            );

        public async Task<TEntity> GetSingleForNameAsync(string name, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Name", name);

            return await GetSingleAsync(options);
        }

        public async Task<TEntity?> GetSingleOrDefaultForNameAsync(string name, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Name", name);

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
