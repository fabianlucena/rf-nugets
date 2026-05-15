using RFBaseEntities.Entities;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
    public class CreatableEntityServiceDecorator<T>(ICreatableEntityService<T> creatableEntityService)
        : EntityServiceDecorator<T>(creatableEntityService),
        ICreatableEntityService<T>
        where T : CreatableEntity, new()
    {
    }
}
