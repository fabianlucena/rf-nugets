using RFEntities.Entities;
using RFIServices.IServices;

namespace RFServices.Decorators
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
