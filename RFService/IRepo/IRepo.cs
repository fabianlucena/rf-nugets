using RFService.ILibs;
using RFService.Repo;
using System.Data;

namespace RFService.IRepo
{
    public interface IRepo<TEntity>
    {
        (IDbConnection, Action) OpenConnection(RepoOptions? options = null);

        Task<TEntity> InsertAsync(TEntity data, RepoOptions? options = null);

        Task<int> GetCountAsync(QueryOptions options);

        Task<TEntity> GetSingleAsync(QueryOptions options);

        Task<TEntity?> GetSingleOrDefaultAsync(QueryOptions options);

        Task<TEntity?> GetFirstOrDefaultAsync(QueryOptions options);

        Task<IEnumerable<TEntity>> GetListAsync(QueryOptions options);

        Task<int> UpdateAsync(IDataDictionary data, QueryOptions options);

        Task<int> DeleteAsync(QueryOptions options);
    }
}
