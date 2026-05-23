using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface IJoinRepository<T>
        : IBaseRepository<T>
        where T : Join, new()
    {
    }
}