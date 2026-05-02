using RFBaseEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseEntities.QueryOptions;

namespace RFBaseIRepositories.IRepositories
{
    public interface IEntityRepository<T>
        : IBaseRepository<T>
        where T : Entity, new()
    {
        Task<IEnumerable<long>> GetIdsAsync(EntityQueryOptions? options = null);
        Task<int> UpdateByIdAsync(long id, IDataDictionary data);
    }
}