using RFService.Entities;
using RFService.ILibs;
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

        public async Task<IEnumerable<Guid>> GetListUuidAsync(QueryOptions options)
            => (await GetListAsync(options)).Select(GetUuid);

        public Task<TEntity> GetSingleForUuidAsync(Guid uuid, QueryOptions? options = null)
            => GetSingleAsync(new QueryOptions(options) { Filters = { { "Uuid", uuid } } });

        public Task<IEnumerable<TEntity>> GetListForUuidsAsync(IEnumerable<Guid> uuids, QueryOptions? options = null)
            => GetListAsync(new QueryOptions(options) { Filters = { { "Uuid", uuids } } });

        public async Task<Int64> GetSingleIdForUuidAsync(Guid uuid, QueryOptions? options = null)
            => GetId(await GetSingleForUuidAsync(uuid, options));

        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Uuid == Guid.Empty)
                data.Uuid = Guid.NewGuid();

            return data;
        }

        public Task<int> UpdateForUuidAsync(IDataDictionary data, Guid uuid, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilterUuid(uuid);
            return UpdateAsync(data, options);
        }
    }
}
