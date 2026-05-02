using RFBaseEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseEntities.QueryOptions;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
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

        public virtual Task UpdateByIdAsync(long id, IDataDictionary data)
            => entityService.UpdateByIdAsync(id, data);
    }
}
