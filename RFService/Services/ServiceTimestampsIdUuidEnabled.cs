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
        public override QueryOptions SanitizeQueryOptions(QueryOptions options)
        {
            if (!options.HasColumnFilter("IsEnabled")
                && !options.GetSwitch("IncludeDisabled", false)
            )
            {
                options = new QueryOptions(options);
                options.AddFilter("IsEnabled", true);
            }

            return base.SanitizeQueryOptions(options);
        }
    }
}
