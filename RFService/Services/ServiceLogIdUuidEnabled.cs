using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceLogIdUuidEnabled<TRepo, TEntity>(TRepo repo)
        : ServiceLogIdUuid<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntityLogIdUuidEnabled
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
