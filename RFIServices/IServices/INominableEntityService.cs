using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices
{
    public interface INominableEntityService<T>
        : ICommonEntityService<T>
        where T : NominableEntity, new()
    {
        Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null);
        Task<T> GetSingleByNameOrCreateAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createData = null);
        Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null);
        Task<long> GetSingleIdByNameAsync(string name, NominableEntityQueryOptions? options = null);
        Task<long> GetSingleIdByNameOrCreateAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createData = null);
        Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, NominableEntityQueryOptions? options = null);
    }
}