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

        Task<IEnumerable<Guid>> GetListUuidAsync(GetOptions options);

        Task<TEntity> GetSingleForUuidAsync(Guid uuid, GetOptions? options = null);

        public async Task<TEntity?> GetSingleOrDefaultForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Uuid", uuid);

            return await GetSingleOrDefaultAsync(options);
        }

        Task<int> UpdateForUuidAsync(Guid uuid, IDataDictionary data, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("uuid", uuid);
            return UpdateAsync(data, options);
        }

        Task<int> DeleteForUuidAsync(Guid uuid, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("uuid", uuid);
            return DeleteAsync(options);
        }
    }
}
