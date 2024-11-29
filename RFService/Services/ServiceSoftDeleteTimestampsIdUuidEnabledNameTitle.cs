using RFService.Entities;
using RFService.IRepo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledNameTitle<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsIdUuidEnabledNameTitle
    {
    }
}
