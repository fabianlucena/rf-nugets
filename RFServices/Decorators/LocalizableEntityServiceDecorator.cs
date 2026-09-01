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
        public virtual Task<T> Translate(T entity, string? context = null)
            => localizableEntityService.Translate(entity, context);

        public virtual Task<T> GetOrCreateByNameAsync(string name, LocalizableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
            => localizableEntityService.GetOrCreateByNameAsync(name, options, createFactory);
    }
}
