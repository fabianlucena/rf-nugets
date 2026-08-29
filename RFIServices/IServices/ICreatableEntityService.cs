using RFEntities.Entities;

namespace RFIServices.IServices;

public interface ICreatableEntityService<T>
    : IEntityService<T>
    where T : CreatableEntity, new()
{
}
