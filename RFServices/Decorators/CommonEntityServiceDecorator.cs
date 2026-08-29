using RFEntities.Entities;
using RFIServices.IServices;

namespace RFServices.Decorators;

public class CommonEntityServiceDecorator<T>(ICommonEntityService<T> commonEntityService)
    : AuditableEntityServiceDecorator<T>(commonEntityService),
    ICommonEntityService<T>
    where T : CommonEntity, new()
{
    public virtual Task<int> RestoreByIdAsync(long id)
        => commonEntityService.RestoreByIdAsync(id);

    public virtual Task<int> RestoreByUuidAsync(Guid uuid)
        => commonEntityService.RestoreByUuidAsync(uuid);
}
