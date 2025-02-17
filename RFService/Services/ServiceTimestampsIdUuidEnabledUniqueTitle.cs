using RFService.Entities;
using RFService.IRepo;
using RFService.IServices;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledUniqueTitle<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsIdUuidEnabledTitle<TRepo, TEntity>(repo),
            IServiceTitle<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuidEnabledUniqueTitle
    {
    }
}
