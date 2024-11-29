using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledNameTitle<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsIdUuidEnabledName<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuidEnabledNameTitle
    {
        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("Title", out object? value))
            {
                if (value == null || string.IsNullOrEmpty((string)value))
                {
                    options = new GetOptions(options);
                    options.Filters.Remove("Title");
                }
            }

            return base.SanitizeForAutoGet(options);
        }
    }
}
