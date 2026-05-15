using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIRepositories.IRepositories
{
    public interface INominableEntityRepository<T>
        : ICommonEntityRepository<T>
        where T : NominableEntity, new()
    {
    }
}