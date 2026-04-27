using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface ICommonJoinRepository<T>
        : ICreatableJoinRepository<T>
        where T : CommonJoin, new()
    {
    }
}