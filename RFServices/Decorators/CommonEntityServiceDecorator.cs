using RFEntities.Entities;
using RFIServices.IServices;
using RFIServices.QueryOptions;

namespace RFServices.Decorators;

public class CommonEntityServiceDecorator<T>(ICommonEntityService<T> commonEntityService)
    : AuditableEntityServiceDecorator<T>(commonEntityService),
    ICommonEntityService<T>
    where T : CommonEntity, new()
{
    public virtual Task<int> DeleteByIdAsync(long id, CommonEntityQueryOptions? options = null)
        => commonEntityService.DeleteByIdAsync(id, options);

    public virtual Task<int> DeleteByUuidAsync(Guid uuid, CommonEntityQueryOptions? options = null)
        => commonEntityService.DeleteByUuidAsync(uuid, options);

    public virtual Task<int> RestoreByIdAsync(long id, CommonEntityQueryOptions? options = null)
        => commonEntityService.RestoreByIdAsync(id, options);

    public virtual Task<int> RestoreByUuidAsync(Guid uuid, CommonEntityQueryOptions? options = null)
        => commonEntityService.RestoreByUuidAsync(uuid, options);
}
