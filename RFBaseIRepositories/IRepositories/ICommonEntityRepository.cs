using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface ICommonEntityRepository<T>
        : IAuditableEntityRepository<T>
        where T : CommonEntity, new()
    {
    }
}