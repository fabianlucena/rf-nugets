using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices;

public interface INominableOwnedEntityService<T>
    : IOwnedEntityService<T>
    where T : NominableOwnedEntity, new()
{
    Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null);
    Task<T> GetSingleOrCreateByNameAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null);
    Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null);
    Task<long> GetSingleIdByNameAsync(string name, NominableEntityQueryOptions? options = null);
    Task<long> GetSingleIdOrCreateByNameAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null);
    Task<IEnumerable<string>> GetNamesAsync(NominableEntityQueryOptions options);
    Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, NominableEntityQueryOptions? options = null);
    Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, NominableEntityQueryOptions? options = null);
}