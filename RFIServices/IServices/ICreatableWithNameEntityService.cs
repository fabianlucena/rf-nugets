using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices;

public interface ICreatableWithNameEntityService<T>
    : ICreatableEntityService<T>
    where T : CreatableWithNameEntity, new()
{
    Task<T?> GetSingleOrDefaultByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null);
    Task<T> GetSingleOrCreateByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null);
    Task<long?> GetSingleIdOrDefaultByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null);
    Task<long> GetSingleIdByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null);
    Task<long> GetSingleIdOrCreateByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null);
    Task<IEnumerable<string>> GetNamesAsync(CreatableWithNameEntityQueryOptions options);
    Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, CreatableWithNameEntityQueryOptions? options = null);
    Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, CreatableWithNameEntityQueryOptions? options = null);
}