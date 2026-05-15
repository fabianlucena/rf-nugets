using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIRepositories.IRepositories
{
    public interface INoIdEntityRepository<T>
        : IBaseRepository<T>
        where T : NoIdEntity, new()
    {
    }
}