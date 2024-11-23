using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabled<Repo, Entity>(Repo repo)
        : ServiceTimestampsIdUuid<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuidEnabled
    {
        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("IsEnabled", out object? value))
            {
                if (value == null)
                {
                    options = new GetOptions(options);
                    options.Filters.Remove("IsEnabled");
                }
            }

            return base.SanitizeForAutoGet(options);
        }
    }
}
