using RFEntities.Entities;
using RFIServices.IServices;
using RFIServices.QueryOptions;

namespace RFServices.Decorators
{
    public class LocalizableEntityServiceDecorator<T>(ILocalizableEntityService<T> localizableEntityService)
        : TitledEntityServiceDecorator<T>(localizableEntityService),
        ILocalizableEntityService<T>
        where T : LocalizableEntity, new()
    {
        public virtual Task<T> Translate(T entity)
            => localizableEntityService.Translate(entity);

        public virtual Task<T> GetSingleByNameOrCreateAsync(string name, LocalizableEntityQueryOptions? options = null, Func<T, Task<T>>? completeCreateData = null)
            => localizableEntityService.GetSingleByNameOrCreateAsync(name, options, completeCreateData);
    }
}
