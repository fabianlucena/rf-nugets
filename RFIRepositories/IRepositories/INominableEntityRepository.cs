using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface INominableEntityRepository<T>
        : ICommonEntityRepository<T>
        where T : NominableEntity, new()
    {
    }
}