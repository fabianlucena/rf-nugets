using RFService.Entities;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceIdUuid<TRepo, TEntity>(TRepo repo)
        : ServiceId<TRepo, TEntity>(repo),
            IServiceIdUuid<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityIdUuid
    {
        public Guid GetUuid(TEntity item) => item.Uuid;

        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Uuid == Guid.Empty)
                data.Uuid = Guid.NewGuid();

            return data;
        }
    }
}
