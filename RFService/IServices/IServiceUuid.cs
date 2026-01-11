using RFService.ILibs;
using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceUuid<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        Guid GetUuid(TEntity item);

        public IDataDictionary SanitizeUuidForAutoGet(IDataDictionary data)
        {
            if (data.TryGetValue("Uuid", out object? value))
            {
                if (value != null
                    && (Guid)value != Guid.Empty
                )
                {
                    return new DataDictionary { { "Uuid", value } };
                }
                else
                {
                    data = new DataDictionary(data);
                    data.Remove("Uuid");
                }
            }

            return data;
        }

        Task<IEnumerable<Guid>> GetListUuidAsync(QueryOptions options);

        Task<IEnumerable<TEntity>> GetListForUuidsAsync(IEnumerable<Guid> uuids, QueryOptions? options = null);

        Task<TEntity> GetSingleForUuidAsync(Guid uuid, QueryOptions? options = null);

        async Task<TEntity?> GetSingleOrDefaultForUuidAsync(Guid uuid, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Uuid", uuid);

            return await GetSingleOrDefaultAsync(options);
        }

        Task<int> UpdateForUuidAsync(IDataDictionary data, Guid uuid, QueryOptions? options = null);

        Task<int> DeleteForUuidAsync(Guid uuid, QueryOptions? options = null, DataDictionary? data = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Uuid", uuid);
            return DeleteAsync(options, data);
        }
    }
}
