using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
    public class NominableEntityServiceDecorator<T>(INominableEntityService<T> nominableEntityService)
        : CommonEntityServiceDecorator<T>(nominableEntityService),
        INominableEntityService<T>
        where T : NominableEntity, new()
    {
        public Task<T?> GetSingleOrDefaultByNameAsync(string name, BaseQueryOptions? options = null)
             => nominableEntityService.GetSingleOrDefaultByNameAsync(name, options);

        public Task<long?> GetSingleOrDefaultIdByNameAsync(string name, BaseQueryOptions? options = null)
             => nominableEntityService.GetSingleOrDefaultIdByNameAsync(name, options);
    }
}
