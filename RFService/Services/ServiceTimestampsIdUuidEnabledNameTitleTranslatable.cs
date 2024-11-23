using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledNameTitleTranslatable<Repo, Entity>(Repo repo)
        : ServiceTimestampsIdUuidEnabledNameTitle<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuidEnabledNameTitleTranslatable
    {
        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("IsTranslatable", out object? value))
            {
                if (value == null)
                {
                    options = new GetOptions(options);
                    options.Filters.Remove("IsTranslatable");
                }
            }

            return base.SanitizeForAutoGet(options);
        }
    }
}
