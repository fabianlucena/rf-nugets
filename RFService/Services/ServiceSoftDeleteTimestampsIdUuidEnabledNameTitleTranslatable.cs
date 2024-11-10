using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable<Repo, Entity>(Repo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabledNameTitle<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntitySoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable
    {
    }
}
