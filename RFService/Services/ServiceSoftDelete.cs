using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDelete<TRepo, TEntity>(TRepo repo)
        : Service<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDelete
    {
        public override GetOptions SanitizeGetOptions(GetOptions options)
        {
            if (!options.Filters.ContainsKey("DeletedAt")
                && (!options.Options.TryGetValue("IncludeDeleted", out object? includeDeletedObj)
                    || includeDeletedObj is not bool includeDeleted
                    || !includeDeleted
                )
            )
            {
                options.Filters["DeletedAt"] = null;
            }

            return base.SanitizeGetOptions(options);
        }

        public override Task<int> DeleteAsync(GetOptions options)
        {
            return UpdateAsync(
                new Dictionary<string, object?>{ { "DeletedAt", DateTime.UtcNow } },
                options
            );
        }
    }
}