using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledNameTitle<Repo, Entity>(Repo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntitySoftDeleteTimestampsIdUuidEnabledNameTitle
    {
    }
}
