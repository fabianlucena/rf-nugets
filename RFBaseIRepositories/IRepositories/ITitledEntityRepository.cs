using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface ITitledEntityRepository<T>
        : INominableEntityRepository<T>
        where T : TitledEntity, new()
    {
    }
}