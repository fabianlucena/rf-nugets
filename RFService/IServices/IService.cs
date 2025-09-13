using RFService.ILibs;
using RFService.Libs;
using RFService.Repo;
using System.Data;

namespace RFService.IServices
{
    public interface IService<TEntity>
        where TEntity : class
    {
        (IDbConnection, Action) OpenConnection(RepoOptions? options = null);

        Task<TEntity> ValidateForCreationAsync(TEntity data);

        Task<TEntity> CreateAsync(TEntity data, QueryOptions? options = null);

        QueryOptions SanitizeQueryOptions(QueryOptions options);

        Task<int> GetCountAsync(QueryOptions options);

        Task<IEnumerable<TEntity>> GetListAsync(QueryOptions options);

        Task<TEntity> GetSingleAsync(QueryOptions options);

        Task<TEntity?> GetSingleOrDefaultAsync(QueryOptions options);

        Task<TEntity?> GetSingleOrCreateAsync(QueryOptions options, Func<TEntity> dataFactory);

        Task<TEntity?> GetFirstOrDefaultAsync(QueryOptions options);

        IDataDictionary SanitizeDataForAutoGet(IDataDictionary data);

        Task<TEntity?> AutoGetFirstOrDefaultAsync(TEntity data);

        Task<TEntity> GetOrCreateAsync(TEntity data);

        Task CreateIfNotExistsAsync(TEntity data);

        Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, QueryOptions options);

        Task<int> UpdateAsync(IDataDictionary data, QueryOptions options);

        Task<int> DeleteAsync(QueryOptions options, DataDictionary? data = null);

        Task<Int64?> GetInt64Async(QueryOptions options);
    }
}
