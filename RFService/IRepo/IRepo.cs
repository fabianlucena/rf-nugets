using RFService.Repo;

namespace RFService.IRepo
{
    public interface IRepo<TEntity>
    {
        Task<TEntity> InsertAsync(TEntity data);

        Task<TEntity> GetSingleAsync(GetOptions options);

        Task<TEntity?> GetSingleOrDefaultAsync(GetOptions options);

        Task<TEntity?> GetFirstOrDefaultAsync(GetOptions options);

        Task<IEnumerable<TEntity>> GetListAsync(GetOptions options);

        Task<IEnumerable<TEntity>> GetListAsync<TIncluded1>(GetOptions options);

        Task<int> UpdateAsync(IDictionary<string, object?> data, GetOptions options);

        Task<int> DeleteAsync(GetOptions options);
    }
}
