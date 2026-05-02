using RFBaseEntities.Entities;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
    public class CommonEntityServiceDecorator<T>(ICommonEntityService<T> commonEntityService)
        : AuditableEntityServiceDecorator<T>(commonEntityService),
        ICommonEntityService<T>
        where T : CommonEntity, new()
    {
    }
}
