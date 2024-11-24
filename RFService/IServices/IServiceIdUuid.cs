using RFService.Exceptions;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceIdUuid<Entity>
        : IServiceUuid<Entity>,
            IServiceId<Entity>
        where Entity : Entities.Entity
    {
        public async Task<Int64> GetIdForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            var item = await GetSingleOrDefaultForUuidAsync(uuid, options);
            if (item == null)
            {
                if (item == null)
                    throw new UuidItemNotFoundException(uuid);

                await CreateAsync(item);
            }

            return GetId(item);
        }

        public async Task<IEnumerable<Int64>> GetListIdForUuidsAsync(Guid[] uuids, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Uuid"] = uuids;

            var rows = await GetListAsync(options);

            return rows.Select(GetId);
        }
    }
}
