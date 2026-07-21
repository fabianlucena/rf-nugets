using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface ICommonEntityRepository<T>
        : IAuditableEntityRepository<T>
        where T : CommonEntity, new()
    {
    }
}