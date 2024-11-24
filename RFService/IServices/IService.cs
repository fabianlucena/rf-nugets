using RFService.Repo;

namespace RFService.IServices
{
    public interface IService<Entity>
        where Entity : class
    {
        Task<Entity> ValidateForCreationAsync(Entity data);

        Task<Entity> CreateAsync(Entity data);

        GetOptions SanitizeGetOptions(GetOptions options);

        Task<IEnumerable<Entity>> GetListAsync(GetOptions options);

        Task<Entity> GetSingleAsync(GetOptions options);

        Task<Entity?> GetSingleOrDefaultAsync(GetOptions options);

        Task<Entity?> GetFirstOrDefaultAsync(GetOptions options);

        GetOptions SanitizeForAutoGet(GetOptions options);

        Task<Entity?> AutoGetFirstOrDefaultAsync(GetOptions options);

        Task<Entity?> AutoGetFirstOrDefaultAsync(Entity data);

        Task<Entity> GetOrCreateAsync(Entity data);

        Task CreateIfNotExistsAsync(Entity data);

        Task<IDictionary<string, object?>> ValidateForUpdateAsync(IDictionary<string, object?> data);

        Task<int> UpdateAsync(IDictionary<string, object?> data, GetOptions options);

        Task<int> DeleteAsync(GetOptions options);
    }
}
