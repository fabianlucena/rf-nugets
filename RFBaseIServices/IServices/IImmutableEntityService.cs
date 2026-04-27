using RFBaseEntities.Entities;

namespace RFBaseIServices.IServices
{
    public interface IImmutableEntityService<T>
        : ICreatableEntityService<T>
        where T : ImmutableEntity, new()
    {
    }
}