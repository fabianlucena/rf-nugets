using RFBaseEntities.Entities;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
    public class LocalizableEntityServiceDecorator<T>(ILocalizableEntityService<T> localizableEntityService)
        : TitledEntityServiceDecorator<T>(localizableEntityService),
        ILocalizableEntityService<T>
        where T : LocalizableEntity, new()
    {
    }
}
