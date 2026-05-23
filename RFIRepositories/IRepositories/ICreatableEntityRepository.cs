using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface ICreatableEntityRepository<T>
        : IEntityRepository<T>
        where T : CreatableEntity, new()
    {
    }
}