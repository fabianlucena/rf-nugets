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
        public virtual Task<IEnumerable<long>> GetListIdAsync(BaseQueryOptions? options = null)
            => entityService.GetListIdAsync(options);

        public virtual Task<T> GetSingleByIdAsync(long id, BaseQueryOptions? options = null)
            => entityService.GetSingleByIdAsync(id, options);

        public virtual Task UpdateByIdAsync(long id, IDataDictionary data)
            => entityService.UpdateByIdAsync(id, data);
    }
}
