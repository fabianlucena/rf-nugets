using RFBase.ILibs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIRepositories.IRepositories;

public interface IEntityRepository<T>
    : IBaseRepository<T>
    where T : Entity, new()
{
    Task<IEnumerable<long>> GetIdsAsync(EntityQueryOptions? options = null);
    Task<long> GetSingleIdOrDefaultByUuidAsync(Guid uuid, EntityQueryOptions? options = null);
    Task<int> UpdateByIdAsync(long id, IDataDictionary data, EntityQueryOptions? options = null);
    Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data, EntityQueryOptions? options = null);
    Task<int> DeleteByIdAsync(long id, EntityQueryOptions? options = null);
    Task<int> DeleteByUuidAsync(Guid uuid, EntityQueryOptions? options = null);
}