using RFBase.ILibs;
using RFEntities.Entities;
using RFIServices.IServices;
using RFIServices.QueryOptions;

namespace RFServices.Decorators;

public class EntityServiceDecorator<T>(IEntityService<T> entityService)
    : BaseServiceDecorator<T>(entityService),
    IEntityService<T>
    where T : Entity, new()
{
    public virtual Task<T?> GetFirstOrDefaultByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
        => entityService.GetFirstOrDefaultByUuidAsync(uuid, options);

    public virtual Task<IEnumerable<long>> GetIdsAsync(EntityQueryOptions options)
        => entityService.GetIdsAsync(options);

    public virtual Task<T> GetSingleByIdAsync(long id, EntityQueryOptions? options = null)
        => entityService.GetSingleByIdAsync(id, options);

    public virtual Task<long> GetSingleIdByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
        => entityService.GetSingleIdByUuidAsync(uuid, options);

    public virtual Task<int> UpdateByIdAsync(long id, IDataDictionary data)
        => entityService.UpdateByIdAsync(id, data);

    public virtual Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data)
        => entityService.UpdateByUuidAsync(uuid, data);

    public virtual Task<int> DeleteByIdAsync(long id)
        => entityService.DeleteByIdAsync(id);

    public virtual Task<int> DeleteByUuidAsync(Guid uuid)
        => entityService.DeleteByUuidAsync(uuid);
}
