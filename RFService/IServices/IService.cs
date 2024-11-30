using RFService.Entities;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IService<TEntity>
        where TEntity : class
    {
        Task<TEntity> ValidateForCreationAsync(TEntity data);

        Task<TEntity> CreateAsync(TEntity data);

        GetOptions SanitizeGetOptions(GetOptions options);

        Task<IEnumerable<TEntity>> GetListAsync(GetOptions options);

        Task<TEntity> GetSingleAsync(GetOptions options);

        Task<TEntity?> GetSingleOrDefaultAsync(GetOptions options);

        Task<TEntity?> GetFirstOrDefaultAsync(GetOptions options);

        GetOptions SanitizeForAutoGet(GetOptions options);

        Task<TEntity?> AutoGetFirstOrDefaultAsync(GetOptions options);

        Task<TEntity?> AutoGetFirstOrDefaultAsync(TEntity data);

        Task<TEntity> GetOrCreateAsync(TEntity data);

        Task CreateIfNotExistsAsync(TEntity data);

        Task<IDictionary<string, object?>> ValidateForUpdateAsync(IDictionary<string, object?> data);

        Task<int> UpdateAsync(IDictionary<string, object?> data, GetOptions options);

        Task<int> DeleteAsync(GetOptions options);
    }
}
