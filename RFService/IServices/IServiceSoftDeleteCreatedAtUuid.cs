using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceSoftDeleteCreatedAtUuid<TEntity>
        : IServiceSoftDeleteCreatedAt<TEntity>
        where TEntity : class
    {
        Task<int> RestoreForUuidAsync(Guid uuid, QueryOptions? options = null, DataDictionary? data = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("uuid", uuid);
            return RestoreAsync(options, data);
        }
    }
}
