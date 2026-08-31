using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface IACommonEntityRepository<T>
        : ICommonEntityRepository<T>
        where T : ACommonEntity, new()
    {
    }
}