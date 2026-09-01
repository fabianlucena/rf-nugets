using RFBase.ILibs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices;

public interface IEntityService<T>
    : IBaseService<T>
    where T : Entity
{
    Task<T?> GetFirstOrDefaultByUuidAsync(Guid uuid, EntityQueryOptions? options = null);
    Task<IEnumerable<long>> GetListIdAsync(EntityQueryOptions options);
    Task<IEnumerable<long>> GetListIdByUuidAsync(IEnumerable<Guid> uuids, EntityQueryOptions? options = null);
    Task<T> GetSingleByIdAsync(long id, EntityQueryOptions? options = null);
    Task<long> GetSingleIdByUuidAsync(Guid uuid, EntityQueryOptions? options = null);
    Task<int> UpdateByIdAsync(long id, IDataDictionary data, EntityQueryOptions? options = null);
    Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data, EntityQueryOptions? options = null);
    Task<int> DeleteByIdAsync(long id, EntityQueryOptions? options = null);
    Task<int> DeleteByUuidAsync(Guid uuid, EntityQueryOptions? options = null);
}
