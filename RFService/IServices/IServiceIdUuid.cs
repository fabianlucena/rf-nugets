using RFService.Entities;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceIdUuid<TEntity>
        : IServiceUuid<TEntity>,
            IServiceId<TEntity>
        where TEntity : Entity
    {
        Task<Int64> GetSingleIdForUuidAsync(Guid uuid, QueryOptions? options = null);

        public async Task<Int64?> GetSingleOrDefaultIdForUuidAsync(Guid uuid, QueryOptions? options = null)
        {
            var item = await GetSingleOrDefaultForUuidAsync(uuid, options);
            if (item == null)
                return null;

            return GetId(item);
        }

        public async Task<IEnumerable<Int64>> GetListIdForUuidsAsync(IEnumerable<Guid> uuids, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Uuid", uuids);

            var rows = await GetListAsync(options);

            return rows.Select(GetId);
        }

        public async Task<Guid> GetSingleUuidForIdAsync(Int64 id, QueryOptions? options = null)
        {
            var item = await GetSingleForIdAsync(id, options);

            return GetUuid(item);
        }
    }
}
