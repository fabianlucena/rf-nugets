using RFBase.Libs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIRepositories.IRepositories
{
    public interface IBaseRepository<T> where T : Base
    {
        IQueryable<T> CreateDBSet(BaseQueryOptions options);
        Task<T> CreateAsync(T entity);
        Task<IEnumerable<T>> GetListAsync(BaseQueryOptions options);
        Task<int> UpdateAsync(DataDictionary data, BaseQueryOptions options);
        Task<int> DeleteAsync(BaseQueryOptions options);
    }
}