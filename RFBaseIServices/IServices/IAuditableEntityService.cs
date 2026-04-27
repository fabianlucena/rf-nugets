using RFBaseEntities.Entities;

namespace RFBaseIServices.IServices
{
    public interface IAuditableEntityService<T>
        : ICreatableEntityService<T>
        where T : AuditableEntity, new()
    {
    }
}