using RFBaseEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseEntities.QueryOptions;

namespace RFBaseIRepositories.IRepositories
{
    public interface IEntityRepository<T>
        : IBaseRepository<T>
        where T : Entity, new()
    {
        Task<IEnumerable<long>> GetListIdAsync(BaseQueryOptions? options = null);
        Task<T> GetSingleByIdAsync(long id, BaseQueryOptions? options = null);
        Task<T?> GetFirstOrDefaultByUuidAsync(Guid uuid);
        Task<bool> UpdateByIdAsync(long id, IDataDictionary data);
    }
}