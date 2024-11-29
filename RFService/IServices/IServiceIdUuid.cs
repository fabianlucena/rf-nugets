using RFService.Exceptions;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceIdUuid<Entity>
        : IServiceUuid<Entity>,
            IServiceId<Entity>
        where Entity : Entities.Entity
    {
        public async Task<Int64> GetSingleIdForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            var item = await GetSingleForUuidAsync(uuid, options);

            return GetId(item);
        }

        public async Task<IEnumerable<Int64>> GetListIdForUuidsAsync(Guid[] uuids, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Uuid"] = uuids;

            var rows = await GetListAsync(options);

            return rows.Select(GetId);
        }

        public async Task<Guid> GetSingleUuidForIdAsync(Int64 id, GetOptions? options = null)
        {
            var item = await GetSingleForIdAsync(id, options);

            return GetUuid(item);
        }
    }
}
