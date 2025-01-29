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

        public async Task<IEnumerable<Guid>> GetListUuidAsync(GetOptions options)
            => (await GetListAsync(options)).Select(GetUuid);

        public Task<TEntity> GetSingleForUuidAsync(Guid uuid, GetOptions? options = null)
            => GetSingleAsync(new GetOptions(options) { Filters = { { "Uuid", uuid } } });

        public async Task<Int64> GetSingleIdForUuidAsync(Guid uuid, GetOptions? options = null)
            => GetId(await GetSingleForUuidAsync(uuid, options));

        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Uuid == Guid.Empty)
                data.Uuid = Guid.NewGuid();

            return data;
        }

        public Task<int> UpdateForUuidAsync(Guid uuid, IDataDictionary data, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilterUuid(uuid);
            return UpdateAsync(data, options);
        }
    }
}
