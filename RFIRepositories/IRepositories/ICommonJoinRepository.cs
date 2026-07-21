using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface ICommonJoinRepository<T>
        : ICreatableJoinRepository<T>
        where T : CommonJoin, new()
    {
    }
}