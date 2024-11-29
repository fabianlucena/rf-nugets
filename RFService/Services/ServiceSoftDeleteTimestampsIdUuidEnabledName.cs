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
    }
}
