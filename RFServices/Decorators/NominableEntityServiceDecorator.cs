using RFEntities.Entities;
using RFIServices.IServices;
using RFIServices.QueryOptions;

namespace RFServices.Decorators
{
    public class NominableEntityServiceDecorator<T>(INominableEntityService<T> nominableEntityService)
        : CommonEntityServiceDecorator<T>(nominableEntityService),
        INominableEntityService<T>
        where T : NominableEntity, new()
    {
        public Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, NominableEntityQueryOptions? options = null)
            => nominableEntityService.GetNamesByIdsAsync(ids, options);

        public Task<T> GetOrCreateByNameAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
            => nominableEntityService.GetOrCreateByNameAsync(name, options, createFactory);

        public Task<long> GetSingleIdByNameAsync(string name, NominableEntityQueryOptions? options = null)
            => nominableEntityService.GetSingleIdByNameAsync(name, options);

        public Task<long> GetIdOrCreateByNameAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
            => nominableEntityService.GetIdOrCreateByNameAsync(name, options, createFactory);

        public Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null)
            => nominableEntityService.GetSingleIdOrDefaultByNameAsync(name, options);

        public Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null)
             => nominableEntityService.GetSingleOrDefaultByNameAsync(name, options);

        public Task<IEnumerable<string>> GetNamesAsync(NominableEntityQueryOptions options)
             => nominableEntityService.GetNamesAsync(options);

        public Task<long?> GetSingleOrDefaultIdByNameAsync(string name, NominableEntityQueryOptions? options = null)
             => nominableEntityService.GetSingleIdOrDefaultByNameAsync(name, options);

        public Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, NominableEntityQueryOptions? options = null)
             => nominableEntityService.GetIdsByNamesAsync(names, options);
    }
}
