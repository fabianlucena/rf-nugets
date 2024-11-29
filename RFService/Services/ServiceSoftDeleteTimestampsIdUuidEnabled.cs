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
