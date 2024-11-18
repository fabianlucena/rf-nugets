using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceUuid<Entity> : IService<Entity>
        where Entity : class
    {
        Task<int> UpdateForUuidAsync(object data, Guid uuid, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["uuid"] = uuid;
            return UpdateAsync(data, options);
        }
    }
}
