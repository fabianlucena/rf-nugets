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

        public Task<T> GetFirstAsync(BaseQueryOptions options)
            => baseService.GetFirstAsync(options);

        public Task<T?> GetFirstOrDefaultAsync(BaseQueryOptions options)
            => baseService.GetFirstOrDefaultAsync(options);

        public virtual Task<IEnumerable<T>> GetListAsync(BaseQueryOptions options)
            => baseService.GetListAsync(options);

        public Task<T> GetSingleAsync(BaseQueryOptions options)
            => baseService.GetSingleAsync(options);

        public Task<T?> GetSingleOrDefaultAsync(BaseQueryOptions options)
            => baseService.GetSingleOrDefaultAsync(options);
    }
}
