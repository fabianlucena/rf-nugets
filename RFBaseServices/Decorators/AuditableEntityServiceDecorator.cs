using RFBaseEntities.Entities;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
    public class AuditableEntityServiceDecorator<T>(IAuditableEntityService<T> auditableEntityService)
        : CreatableEntityServiceDecorator<T>(auditableEntityService),
        IAuditableEntityService<T>
        where T : AuditableEntity, new()
    {
    }
}
