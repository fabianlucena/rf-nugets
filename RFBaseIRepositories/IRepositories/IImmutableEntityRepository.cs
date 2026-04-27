using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface IImmutableEntityRepository<T>
        : ICreatableEntityRepository<T>
        where T : ImmutableEntity, new()
    {
    }
}