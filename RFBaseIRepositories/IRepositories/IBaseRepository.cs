using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIRepositories.IRepositories
{
    public interface IBaseRepository<T> where T : Base
    {
        IQueryable<T> CreateDBSet(BaseQueryOptions? options = null);
        Task<T> CreateAsync(T entity);
        Task<IEnumerable<T>> GetListAsync(BaseQueryOptions? options = null);
    }
}