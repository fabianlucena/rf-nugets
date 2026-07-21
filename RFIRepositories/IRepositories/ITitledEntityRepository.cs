using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface ITitledEntityRepository<T>
        : INominableEntityRepository<T>
        where T : TitledEntity, new()
    {
    }
}