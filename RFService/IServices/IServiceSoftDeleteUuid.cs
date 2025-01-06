using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceSoftDeleteUuid<TEntity>
        : IServiceSoftDelete<TEntity>
        where TEntity : class
    {
        Task<int> RestoreForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("uuid", uuid);
            return RestoreAsync(options);
        }
    }
}
