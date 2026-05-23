using RFEntities.Entities;

namespace RFIServices.IServices
{
    public interface IAuditableEntityService<T>
        : ICreatableEntityService<T>
        where T : AuditableEntity, new()
    {
    }
}