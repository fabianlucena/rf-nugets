using RFBaseEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseEntities.QueryOptions;

namespace RFBaseIServices.IServices
{
    public interface IEntityService<T>
        : IBaseService<T>
        where T : Entity
    {
        Task<T?> GetFirstOrDefaultByUuidAsync(Guid uuid, EntityQueryOptions? options = null);
        Task<IEnumerable<long>> GetIdsAsync(EntityQueryOptions options);
        Task<T> GetSingleByIdAsync(long id, EntityQueryOptions? options = null);
        Task UpdateByIdAsync(long id, IDataDictionary data);
    }
}
