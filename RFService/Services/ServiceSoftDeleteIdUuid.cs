using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteIdUuid<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteId<TRepo, TEntity>(repo),
            IServiceUuid<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteIdUuid
    {
        public Guid GetUuid(TEntity item) => item.Uuid;

        public async Task<IEnumerable<Guid>> GetListUuidAsync(GetOptions options)
            => (await GetListAsync(options)).Select(GetUuid);

        public Task<TEntity> GetSingleForUuidAsync(Guid uuid, GetOptions? options = null)
            => GetSingleAsync(new GetOptions(options) { Filters = { { "Uuid", uuid } } });

        public Task<IEnumerable<TEntity>> GetListForUuidsAsync(IEnumerable<Guid> uuids, GetOptions? options = null)
            => GetListAsync(new GetOptions(options) { Filters = { { "Uuid", uuids } } });

        public async Task<Int64> GetSingleIdForUuidAsync(Guid uuid, GetOptions? options = null)
            => GetId(await GetSingleForUuidAsync(uuid, options));

        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Uuid == Guid.Empty)
            {
                data.Uuid = Guid.NewGuid();
            }

            return data;
        }

        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceUuid<TEntity>)this).SanitizeUuidForAutoGet(data)
            );

        public Task<int> UpdateForUuidAsync(IDataDictionary data, Guid uuid, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilterUuid(uuid);
            return UpdateAsync(data, options);
        }
    }
}
