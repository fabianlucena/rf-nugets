using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestamps<TRepo, TEntity>(TRepo repo)
        : Service<TRepo, TEntity>(repo),
            IServiceTimestamps<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestamps
    {
        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            data.CreatedAt = DateTime.UtcNow;
            data.UpdatedAt = DateTime.UtcNow;

            return data;
        }

        public override async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, QueryOptions options)
        {
            data = await base.ValidateForUpdateAsync(data, options);

            if (!data.ContainsKey("UpdatedAt")
                && !data.ContainsKey("DeletedAt")
            )
            {
                data["UpdatedAt"] = DateTime.UtcNow;
            }

            return data;
        }

        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceTimestamps<TEntity>)this).SanitizeTimestampsForAutoGet(data)
            );
    }
}