using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceId<Entity> : IService<Entity>
        where Entity : class
    {
        Int64 GetId(Entity item);

        Task<Entity> GetForIdAsync(Int64 id, GetOptions? options = null);

        Task<Entity?> GetSingleOrDefaultForIdAsync(Int64 id, GetOptions? options = null);

        Task<int> UpdateForIdAsync(object data, Int64 id, GetOptions? options = null);

        Task<int> DeleteForIdAsync(Int64 id, GetOptions? options = null);
    }
}
