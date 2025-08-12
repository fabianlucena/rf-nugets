using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceSoftDeleteId<TEntity>
        : IServiceSoftDelete<TEntity>
        where TEntity : class
    {
        Task<int> RestoreForIdAsync(Int64 id, QueryOptions? options = null, DataDictionary? data = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Id", id);
            return RestoreAsync(options, data);
        }
    }
}
