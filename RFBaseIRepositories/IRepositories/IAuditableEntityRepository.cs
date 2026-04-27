using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface IAuditableEntityRepository<T>
        : ICreatableEntityRepository<T>
        where T : AuditableEntity, new()
    {
    }
}