using RFBase.Libs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices;

public interface IBaseService<T> where T : Base
{
    Task<T> CreateAsync(T entity);
    Task<IEnumerable<T>> GetListAsync(BaseQueryOptions options);
    Task<T> GetFirstAsync(BaseQueryOptions options);
    Task<T?> GetFirstOrDefaultAsync(BaseQueryOptions options);
    Task<T> GetSingleAsync(BaseQueryOptions options);
    Task<T?> GetSingleOrDefaultAsync(BaseQueryOptions options);

    Task<int> UpdateAsync(DataDictionary data, BaseQueryOptions options);
    Task<int> DeleteAsync(BaseQueryOptions options);
}
