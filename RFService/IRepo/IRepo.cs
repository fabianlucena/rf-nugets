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

        Task<IEnumerable<TEntity>> GetListAsync(QueryOptions options);

        Task<int> UpdateAsync(IDataDictionary data, QueryOptions options);

        Task<int> DeleteAsync(QueryOptions options);

        Task<Int64?> GetInt64Async(QueryOptions options);
    }
}
