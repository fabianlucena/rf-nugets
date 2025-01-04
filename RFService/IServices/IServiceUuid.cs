using RFService.ILibs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceUuid<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        Guid GetUuid(TEntity item);

        Task<IEnumerable<Guid>> GetListUuidAsync(GetOptions options);

        public async Task<TEntity> GetSingleForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Uuid"] = uuid;

            return await GetSingleAsync(options);
        }

        public async Task<TEntity?> GetSingleOrDefaultForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Uuid"] = uuid;

            return await GetSingleOrDefaultAsync(options);
        }

        Task<int> UpdateForUuidAsync(Guid uuid, IDataDictionary data, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["uuid"] = uuid;
            return UpdateAsync(data, options);
        }

        Task<int> DeleteForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["uuid"] = uuid;
            return DeleteAsync(options);
        }
    }
}
