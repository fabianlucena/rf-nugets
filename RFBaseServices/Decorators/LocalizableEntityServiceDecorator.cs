using RFBaseEntities.Entities;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
    public class LocalizableEntityServiceDecorator<T>(ILocalizableEntityService<T> localizableEntityService)
        : TitledEntityServiceDecorator<T>(localizableEntityService),
        ILocalizableEntityService<T>
        where T : LocalizableEntity, new()
    {
        public virtual Task<T> Translate(T entity)
            => localizableEntityService.Translate(entity);
    }
}
