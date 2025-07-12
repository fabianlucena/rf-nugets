using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteCreatedAt<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDelete<TRepo, TEntity>(repo),
            IServiceSoftDelete<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteCreatedAt
    {
        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            data.CreatedAt = DateTime.UtcNow;

            return data;
        }

        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceSoftDeleteCreatedAt<TEntity>)this).SanitizeSoftDeleteCreatedAtForAutoGet(data)
            );
    }
}