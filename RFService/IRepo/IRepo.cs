using RFService.Repo;
using System.Data;

namespace RFService.IRepo
{
    public interface IRepo<TEntity>
    {
        (IDbConnection, Action) OpenConnection(RepoOptions? options = null);

        Task<TEntity> InsertAsync(TEntity data, RepoOptions? options = null);

        Task<TEntity> GetSingleAsync(GetOptions options);

        Task<TEntity?> GetSingleOrDefaultAsync(GetOptions options);

        Task<TEntity?> GetFirstOrDefaultAsync(GetOptions options);

        Task<IEnumerable<TEntity>> GetListAsync(GetOptions options);

        Task<int> UpdateAsync(IDictionary<string, object?> data, GetOptions options);

        Task<int> DeleteAsync(GetOptions options);
    }
}
