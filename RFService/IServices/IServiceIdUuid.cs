using RFService.Entities;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceIdUuid<TEntity>
        : IServiceUuid<TEntity>,
            IServiceId<TEntity>
        where TEntity : Entity
    {
        public async Task<Int64> GetSingleIdForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            var item = await GetSingleForUuidAsync(uuid, options);

            return GetId(item);
        }

        public async Task<Int64?> GetSingleOrDefaultIdForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            var item = await GetSingleOrDefaultForUuidAsync(uuid, options);
            if (item == null)
                return null;

            return GetId(item);
        }

        public async Task<IEnumerable<Int64>> GetListIdForUuidsAsync(IEnumerable<Guid> uuids, GetOptions? options = null)
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
