using RFService.Entities;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledName<Repo, Entity>(Repo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabled<Repo, Entity>(repo),
            IServiceName<Entity>,
            IServiceIdName<Entity>
        where Repo : IRepo<Entity>
        where Entity : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            return base.SanitizeForAutoGet(
                ((IServiceName<Entity>)this).SanitizeNameForAutoGet(options)
            );
        }
    }
}
