using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface IAuditableEntityRepository<T>
        : ICreatableEntityRepository<T>
        where T : AuditableEntity, new()
    {
    }
}