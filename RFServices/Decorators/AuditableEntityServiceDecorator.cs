using RFEntities.Entities;
using RFIServices.IServices;

namespace RFServices.Decorators;

public class AuditableEntityServiceDecorator<T>(IAuditableEntityService<T> auditableEntityService)
    : CreatableEntityServiceDecorator<T>(auditableEntityService),
    IAuditableEntityService<T>
    where T : AuditableEntity, new()
{
}
