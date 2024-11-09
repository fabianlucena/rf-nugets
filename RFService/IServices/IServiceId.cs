using RFService.Repo;

namespace RFService.IService
{
    public interface IServiceId<Entity> : IService<Entity>
        where Entity : class
    {
        Task<Entity> GetForIdAsync(Int64 id, GetOptions? options = null);

        Task<Entity?> GetSingleOrDefaultForIdAsync(Int64 id, GetOptions? options = null);

        Task<int> UpdateForIdAsync(object data, Int64 id, GetOptions? options = null);

        Task<int> DeleteForIdAsync(Int64 id, GetOptions? options = null);
    }
}
