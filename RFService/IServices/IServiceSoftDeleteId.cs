using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceSoftDeleteId<TEntity>
        : IServiceSoftDelete<TEntity>
        where TEntity : class
    {
        Task<int> RestoreForIdAsync(Int64 id, GetOptions? options = null, DataDictionary? data = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Id", id);
            return RestoreAsync(options, data);
        }
    }
}
