using RFBase.ILibs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIRepositories.IRepositories;

public interface IBaseRepository<T> where T : Base
{
    Task<T> CreateAsync(T entity);
    Task<IEnumerable<T>> GetListAsync(BaseQueryOptions options);
    Task<int> GetCountAsync(BaseQueryOptions options);
    Task<int> UpdateAsync(IDataDictionary data, BaseQueryOptions options);
    Task<int> DeleteAsync(BaseQueryOptions options);
}