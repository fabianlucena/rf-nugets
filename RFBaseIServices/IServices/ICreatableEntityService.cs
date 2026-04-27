using RFBaseEntities.Entities;

namespace RFBaseIServices.IServices
{
    public interface ICreatableEntityService<T>
        : IEntityService<T>
        where T : CreatableEntity, new()
    {
    }
}
