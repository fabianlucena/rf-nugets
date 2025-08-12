using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteLogIdUuidEnabled<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteLogIdUuid<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteLogIdUuidEnabled
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
