using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledNameTitleTranslatable<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsIdUuidEnabledNameTitle<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuidEnabledNameTitleTranslatable
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
