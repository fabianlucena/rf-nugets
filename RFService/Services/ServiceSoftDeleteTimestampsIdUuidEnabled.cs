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
        public override QueryOptions SanitizeQueryOptions(QueryOptions options)
        {
            if (!options.HasColumnFilter("IsEnabled")
                && !options.IncludeDisabled
            )
            {
                options = new QueryOptions(options);
                options.AddFilter("IsEnabled", true);
            }

            return base.SanitizeQueryOptions(options);
        }
    }
}
