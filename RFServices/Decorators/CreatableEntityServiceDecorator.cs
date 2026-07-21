using RFEntities.Entities;
using RFIServices.IServices;

namespace RFServices.Decorators
{
    public class CreatableEntityServiceDecorator<T>(ICreatableEntityService<T> creatableEntityService)
        : EntityServiceDecorator<T>(creatableEntityService),
        ICreatableEntityService<T>
        where T : CreatableEntity, new()
    {
    }
}
