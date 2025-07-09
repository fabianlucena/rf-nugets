using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceSoftDeleteUuid<TEntity>
        : IServiceSoftDelete<TEntity>
        where TEntity : class
    {
        Task<int> RestoreForUuidAsync(Guid uuid, GetOptions? options = null, DataDictionary? data = null)
        {
            options ??= new GetOptions();
            options.AddFilter("uuid", uuid);
            return RestoreAsync(options, data);
        }
    }
}
