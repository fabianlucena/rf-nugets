using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;

namespace RFService.Services
{
    public abstract class ServiceCreatedAt<TRepo, TEntity>(TRepo repo)
        : Service<TRepo, TEntity>(repo),
            IService<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityCreatedAt
    {
        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            data.CreatedAt = DateTime.UtcNow;

            return data;
        }

        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceCreatedAt<TEntity>)this).SanitizeCreatedAtForAutoGet(data)
            );
    }
}