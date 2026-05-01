using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
    public class BaseServiceDecorator<T>(IBaseService<T> baseService)
        : IBaseService<T>
        where T : Base, new()
    {
        public virtual Task<T> CreateAsync(T entity)
            => baseService.CreateAsync(entity);

        public virtual Task<IEnumerable<T>> GetListAsync(BaseQueryOptions? options = null)
            => baseService.GetListAsync(options);
    }
}
