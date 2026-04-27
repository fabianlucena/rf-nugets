using RFBaseEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseEntities.QueryOptions;

namespace RFBaseIServices.IServices
{
    public interface IEntityService<T>
        : IBaseService<T>
        where T : Entity
    {
        Task<IEnumerable<long>> GetListIdAsync(BaseQueryOptions? options = null);
        Task<T> GetSingleByIdAsync(long id, BaseQueryOptions? options = null);
        Task UpdateByIdAsync(long id, IDataDictionary data);
    }
}
