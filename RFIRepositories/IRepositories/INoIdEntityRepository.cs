using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface INoIdEntityRepository<T>
        : IBaseRepository<T>
        where T : NoIdEntity, new()
    {
    }
}