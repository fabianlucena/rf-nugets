using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices;

public interface INominableOwnedEntityService<T>
    : IOwnedEntityService<T>
    where T : NominableOwnedEntity, new()
{
    Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null);
    Task<T> GetSingleOrCreateByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null);
    Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null);
    Task<long> GetSingleIdByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null);
    Task<long> GetSingleIdOrCreateByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null);
    Task<IEnumerable<string>> GetNamesAsync(NominableOwnedEntityQueryOptions options);
    Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, NominableOwnedEntityQueryOptions? options = null);
    Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, NominableOwnedEntityQueryOptions? options = null);
}