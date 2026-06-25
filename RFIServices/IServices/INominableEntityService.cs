using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices
{
    public interface INominableEntityService<T>
        : ICommonEntityService<T>
        where T : NominableEntity, new()
    {
        Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null);
        Task<T> GetOrCreateByNameAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null);
        Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null);
        Task<long> GetSingleIdByNameAsync(string name, NominableEntityQueryOptions? options = null);
        Task<long> GetIdOrCreateByNameAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null);
        Task<IEnumerable<string>> GetNamesAsync(NominableEntityQueryOptions options);
        Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, NominableEntityQueryOptions? options = null);
    }
}