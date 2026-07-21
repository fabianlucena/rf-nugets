using RFEntities.Entities;

namespace RFIServices.IServices
{
    public interface IImmutableEntityService<T>
        : ICreatableEntityService<T>
        where T : ImmutableEntity, new()
    {
    }
}