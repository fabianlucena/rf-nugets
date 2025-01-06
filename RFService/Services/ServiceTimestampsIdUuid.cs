using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuid<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsId<TRepo, TEntity>(repo),
            IServiceUuid<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuid
    {
        public Guid GetUuid(TEntity item) => item.Uuid;

        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceUuid<TEntity>)this).SanitizeUuidForAutoGet(data)
            );

        public async Task<IEnumerable<Guid>> GetListUuidAsync(GetOptions options)
            => (await GetListAsync(options)).Select(GetUuid);

        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Uuid == Guid.Empty)
            {
                data.Uuid = Guid.NewGuid();
            }

            return data;
        }
    }
}
