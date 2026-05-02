using RFBaseEntities.Entities;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
    public class TranslatableEntityServiceDecorator<T>(ITranslatableEntityService<T> translatableEntityService)
        : CommonEntityServiceDecorator<T>(translatableEntityService),
        ITranslatableEntityService<T>
        where T : TranslatableEntity, new()
    {
    }
}
