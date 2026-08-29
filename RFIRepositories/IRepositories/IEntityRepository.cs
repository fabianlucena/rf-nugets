using RFBase.ILibs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIRepositories.IRepositories;

public interface IEntityRepository<T>
    : IBaseRepository<T>
    where T : Entity, new()
{
    Task<IEnumerable<long>> GetIdsAsync(EntityQueryOptions? options = null);
    Task<int> UpdateByIdAsync(long id, IDataDictionary data);
    Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data);
    Task<int> DeleteByIdAsync(long id);
    Task<int> DeleteByUuidAsync(Guid uuid);
}