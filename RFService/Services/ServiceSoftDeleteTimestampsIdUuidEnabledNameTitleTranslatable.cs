using RFService.Entities;
using RFService.IRepo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabledNameTitle<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable
    {
    }
}
