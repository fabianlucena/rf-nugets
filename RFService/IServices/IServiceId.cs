using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceId<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        Int64 GetId(TEntity item);

        public async Task<IEnumerable<Int64>> GetListIdAsync(GetOptions options)
            => (await GetListAsync(options)).Select(GetId);

        Task<TEntity> GetSingleForIdAsync(Int64 id, GetOptions? options = null);

        Task<TEntity?> GetSingleOrDefaultForIdAsync(Int64 id, GetOptions? options = null);

        Task<IEnumerable<TEntity>> GetListForIdsAsync(IEnumerable<Int64> id, GetOptions? options = null);

        Task<int> UpdateForIdAsync(IDictionary<string, object?> data, Int64 id, GetOptions? options = null);

        Task<int> DeleteForIdAsync(Int64 id, GetOptions? options = null);
    }
}
