using RFBase.ILibs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices
{
    public interface IEntityService<T>
        : IBaseService<T>
        where T : Entity
    {
        Task<T?> GetFirstOrDefaultByUuidAsync(Guid uuid, EntityQueryOptions? options = null);
        Task<IEnumerable<long>> GetIdsAsync(EntityQueryOptions options);
        Task<T> GetSingleByIdAsync(long id, EntityQueryOptions? options = null);
        Task<int> UpdateByIdAsync(long id, IDataDictionary data);
        Task<int> DeleteByIdAsync(long id);
    }
}
