using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabled<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestampsIdUuid<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsIdUuidEnabled
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
