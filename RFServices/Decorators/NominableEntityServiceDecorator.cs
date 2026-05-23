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
        public Task<long> GetSingleIdByNameAsync(string name, NominableEntityQueryOptions? options = null)
            => nominableEntityService.GetSingleIdByNameAsync(name, options);

        public Task<long> GetSingleIdByNameOrCreateAsync(string name, NominableEntityQueryOptions? options = null, Func<Task<T>, T>? completeCreateData = null)
            => nominableEntityService.GetSingleIdByNameOrCreateAsync(name, options, completeCreateData);

        public Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null)
            => nominableEntityService.GetSingleIdOrDefaultByNameAsync(name, options);

        public Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null)
             => nominableEntityService.GetSingleOrDefaultByNameAsync(name, options);

        public Task<long?> GetSingleOrDefaultIdByNameAsync(string name, NominableEntityQueryOptions? options = null)
             => nominableEntityService.GetSingleIdOrDefaultByNameAsync(name, options);
    }
}
