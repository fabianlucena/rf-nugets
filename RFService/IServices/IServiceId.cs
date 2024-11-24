using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceId<Entity>
        : IService<Entity>
        where Entity : class
    {
        Int64 GetId(Entity item);

        public async Task<IEnumerable<Int64>> GetListIdAsync(GetOptions options)
            => (await GetListAsync(options)).Select(GetId);

        Task<Entity> GetForIdAsync(Int64 id, GetOptions? options = null);

        Task<Entity?> GetSingleOrDefaultForIdAsync(Int64 id, GetOptions? options = null);

        Task<int> UpdateForIdAsync(IDictionary<string, object?> data, Int64 id, GetOptions? options = null);

        Task<int> DeleteForIdAsync(Int64 id, GetOptions? options = null);
    }
}
