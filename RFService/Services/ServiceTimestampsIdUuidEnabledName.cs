using RFService.Entities;
using RFService.IRepo;
using RFService.IServices;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledName<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsIdUuidEnabled<TRepo, TEntity>(repo),
            IServiceName<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuidEnabledName
    {
        
    }
}
