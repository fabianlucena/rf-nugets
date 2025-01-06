using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabled<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsIdUuid<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuidEnabled
    {
        public override GetOptions SanitizeGetOptions(GetOptions options)
        {
            if (!options.HasColumnFilter("IsEnabled")
                && (!options.Options.TryGetBool("IncludeDisabled", out bool includeDisabled) || !includeDisabled)
            )
            {
                options = new GetOptions(options);
                options.AddFilter("IsEnabled", true);
            }

            return base.SanitizeGetOptions(options);
        }
    }
}
