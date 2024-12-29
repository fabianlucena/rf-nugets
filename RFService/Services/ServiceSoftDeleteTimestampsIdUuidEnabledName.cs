using RFService.Entities;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledName<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabled<TRepo, TEntity>(repo),
            IServiceName<TEntity>,
            IServiceIdName<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            return base.SanitizeForAutoGet(
                ((IServiceName<TEntity>)this).SanitizeNameForAutoGet(options)
            );
        }

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
