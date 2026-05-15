using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface ICreatableJoinRepository<T>
        : IJoinRepository<T>
        where T : CreatableJoin, new()
    {
    }
}