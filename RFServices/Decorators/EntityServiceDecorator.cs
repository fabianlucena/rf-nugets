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
    public virtual Task<T> GetSingleByIdAsync(long id, EntityQueryOptions? options = null)
        => entityService.GetSingleByIdAsync(id, options);

    public virtual Task<T?> GetSingleOrDefaultByIdAsync(long id, EntityQueryOptions? options = null)
        => entityService.GetSingleOrDefaultByIdAsync(id, options);

    public virtual Task<T> GetSingleByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
        => entityService.GetSingleByUuidAsync(uuid, options);

    public virtual Task<T?> GetFirstOrDefaultByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
        => entityService.GetFirstOrDefaultByUuidAsync(uuid, options);
    public virtual Task<T?> GetSingleOrDefaultByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
        => entityService.GetSingleOrDefaultByUuidAsync(uuid, options);

    public virtual Task<IEnumerable<long>> GetListIdAsync(EntityQueryOptions options)
        => entityService.GetListIdAsync(options);

    public virtual Task<IEnumerable<Guid>> GetListUuidAsync(EntityQueryOptions options)
        => entityService.GetListUuidAsync(options);

    public virtual Task<IEnumerable<long>> GetListIdByUuidAsync(IEnumerable<Guid> uuids, EntityQueryOptions? options = null)
        => entityService.GetListIdByUuidAsync(uuids, options);

    public virtual Task<long> GetSingleIdByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
        => entityService.GetSingleIdByUuidAsync(uuid, options);

    public virtual Task<int> UpdateByIdAsync(long id, IDataDictionary data, EntityQueryOptions? options = null)
        => entityService.UpdateByIdAsync(id, data, options);

    public virtual Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data, EntityQueryOptions? options = null)
        => entityService.UpdateByUuidAsync(uuid, data, options);

    public virtual Task<int> DeleteByIdAsync(long id, EntityQueryOptions? options = null)
        => entityService.DeleteByIdAsync(id, options);

    public virtual Task<int> DeleteByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
        => entityService.DeleteByUuidAsync(uuid, options);
}
