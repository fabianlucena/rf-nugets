using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIServices.IServices
{
    public interface IBaseService<T> where T : Base
    {
        Task<T> CreateAsync(T entity);
        Task<IEnumerable<T>> GetListAsync(BaseQueryOptions? options = null);
    }
}
