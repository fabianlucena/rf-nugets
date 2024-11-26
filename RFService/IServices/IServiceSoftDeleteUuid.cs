using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceSoftDeleteUuid<Entity>
        : IServiceSoftDelete<Entity>
        where Entity : class
    {
        Task<int> RestoreForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["uuid"] = uuid;
            return RestoreAsync(options);
        }
    }
}
