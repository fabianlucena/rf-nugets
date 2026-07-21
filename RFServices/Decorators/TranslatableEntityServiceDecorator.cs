using RFEntities.Entities;
using RFIServices.IServices;

namespace RFServices.Decorators
{
    public class TranslatableEntityServiceDecorator<T>(ITranslatableEntityService<T> translatableEntityService)
        : CommonEntityServiceDecorator<T>(translatableEntityService),
        ITranslatableEntityService<T>
        where T : TranslatableEntity, new()
    {
    }
}
