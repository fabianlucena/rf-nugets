using RFService.ILibs;
using RFService.Repo;
using System.Data;

namespace RFService.IServices
{
    public interface IService<TEntity>
        where TEntity : class
    {
        (IDbConnection, Action) OpenConnection(RepoOptions? options = null);

        Task<TEntity> ValidateForCreationAsync(TEntity data);

        Task<TEntity> CreateAsync(TEntity data, GetOptions? options = null);

        GetOptions SanitizeGetOptions(GetOptions options);

        Task<int> GetCountAsync(GetOptions options);

        Task<IEnumerable<TEntity>> GetListAsync(GetOptions options);

        Task<TEntity> GetSingleAsync(GetOptions options);

        Task<TEntity?> GetSingleOrDefaultAsync(GetOptions options);

        Task<TEntity?> GetFirstOrDefaultAsync(GetOptions options);

        IDataDictionary SanitizeDataForAutoGet(IDataDictionary data);

        Task<TEntity?> AutoGetFirstOrDefaultAsync(TEntity data);

        Task<TEntity> GetOrCreateAsync(TEntity data);

        Task CreateIfNotExistsAsync(TEntity data);

        Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, GetOptions options);

        Task<int> UpdateAsync(IDataDictionary data, GetOptions options);

        Task<int> DeleteAsync(GetOptions options);
    }
}
