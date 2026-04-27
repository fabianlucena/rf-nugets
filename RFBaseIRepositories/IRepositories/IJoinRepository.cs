using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface IJoinRepository<T>
        : IBaseRepository<T>
        where T : Join, new()
    {
    }
}