using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface ICreatableJoinRepository<T>
        : IJoinRepository<T>
        where T : CreatableJoin, new()
    {
    }
}