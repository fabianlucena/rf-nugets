using RFService.Entities;
using RFService.Exceptions;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledUniqueTitle<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabledTitle<TRepo, TEntity>(repo),
            IServiceUniqueTitle<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsIdUuidEnabledUniqueTitle
    {
    }
}
