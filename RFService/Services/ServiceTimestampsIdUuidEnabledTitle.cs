using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledTitle<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsIdUuidEnabled<TRepo, TEntity>(repo),
            IServiceTitle<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuidEnabledTitle
    {
        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceTitle<TEntity>)this).SanitizeTitleForAutoGet(data)
            );
    }
}
