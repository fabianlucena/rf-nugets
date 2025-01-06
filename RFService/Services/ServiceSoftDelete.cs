using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;
using RFService.Libs;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDelete<TRepo, TEntity>(TRepo repo)
        : Service<TRepo, TEntity>(repo),
            IServiceSoftDelete<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDelete
    {
        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceSoftDelete<TEntity>)this).SanitizeSoftDeleteForAutoGet(data)
            );

        public override GetOptions SanitizeGetOptions(GetOptions options)
        {
            if (!options.HasColumnFilter("DeletedAt")
                && (!options.Options.TryGetValue("IncludeDeleted", out object? includeDeletedObj)
                    || includeDeletedObj is not bool includeDeleted
                    || !includeDeleted
                )
            )
            {
                options.AddFilter("DeletedAt", null);
            }

            return base.SanitizeGetOptions(options);
        }

        public override Task<int> DeleteAsync(GetOptions options)
        {
            return UpdateAsync(
                new DataDictionary { { "DeletedAt", DateTime.UtcNow } },
                options
            );
        }
    }
}