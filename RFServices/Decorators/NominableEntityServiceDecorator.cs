using RFEntities.Entities;
using RFIServices.QueryOptions;
using RFIServices.IServices;

namespace RFServices.Decorators
{
    public class NominableEntityServiceDecorator<T>(INominableEntityService<T> nominableEntityService)
        : CommonEntityServiceDecorator<T>(nominableEntityService),
        INominableEntityService<T>
        where T : NominableEntity, new()
    {
        public Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, NominableEntityQueryOptions? options = null)
            => nominableEntityService.GetNamesByIdsAsync(ids, options);

        public Task<T> GetSingleByNameOrCreateAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createData = null)
            => nominableEntityService.GetSingleByNameOrCreateAsync(name, options, createData);

        public Task<long> GetSingleIdByNameAsync(string name, NominableEntityQueryOptions? options = null)
            => nominableEntityService.GetSingleIdByNameAsync(name, options);

        public Task<long> GetSingleIdByNameOrCreateAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? completeCreateData = null)
            => nominableEntityService.GetSingleIdByNameOrCreateAsync(name, options, completeCreateData);

        public Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null)
            => nominableEntityService.GetSingleIdOrDefaultByNameAsync(name, options);

        public Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null)
             => nominableEntityService.GetSingleOrDefaultByNameAsync(name, options);

        public Task<long?> GetSingleOrDefaultIdByNameAsync(string name, NominableEntityQueryOptions? options = null)
             => nominableEntityService.GetSingleIdOrDefaultByNameAsync(name, options);
    }
}
