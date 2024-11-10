using RFService.Entities;
using RFService.Exceptions;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledName<Repo, Entity>(Repo repo)
        : ServiceTimestampsIdUuidEnabled<Repo, Entity>(repo),
            IServiceName<Entity>
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuidEnabledName
    {
        
    }
}
