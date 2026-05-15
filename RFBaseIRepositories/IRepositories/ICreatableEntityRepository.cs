using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface ICreatableEntityRepository<T>
        : IEntityRepository<T>
        where T : CreatableEntity, new()
    {
    }
}