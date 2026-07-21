using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface IImmutableEntityRepository<T>
        : ICreatableEntityRepository<T>
        where T : ImmutableEntity, new()
    {
    }
}