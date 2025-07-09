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

        public override QueryOptions SanitizeQueryOptions(QueryOptions options)
        {
            if (!options.HasColumnFilter("DeletedAt")
                && !options.IncludeDeleted
            )
            {
                options = new QueryOptions(options);
                options.AddFilter("DeletedAt", null);
            }

            return base.SanitizeQueryOptions(options);
        }

        public override Task<int> DeleteAsync(QueryOptions options, DataDictionary? data = null)
        {
            data ??= [];
            data["DeletedAt"] = DateTime.UtcNow;
            return UpdateAsync(data, options);
        }

        public Task<int> RestoreAsync(QueryOptions options, DataDictionary? data = null)
        {
            data ??= [];
            data["DeletedAt"] = null;
            return UpdateAsync(data, options);
        }
    }
}