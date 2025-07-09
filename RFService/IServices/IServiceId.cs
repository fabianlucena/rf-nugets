using RFService.ILibs;
using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceId<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        Int64 GetId(TEntity item);

        public IDataDictionary SanitizeIdForAutoGet(IDataDictionary data)
        {
            if (data.TryGetValue("Id", out object? value))
            {
                if (value != null
                    && (Int64)value > 0
                )
                {
                    return new DataDictionary { { "Id", value } };
                }
                else
                {
                    data = new DataDictionary(data);
                    data.Remove("Id");
                }
            }

            return data;
        }

        Task<IEnumerable<Int64>> GetListIdAsync(QueryOptions options);

        Task<TEntity> GetSingleForIdAsync(Int64 id, QueryOptions? options = null);

        Task<Int64> GetSingleIdAsync(QueryOptions? options = null);

        Task<TEntity?> GetSingleOrDefaultForIdAsync(Int64 id, QueryOptions? options = null);

        Task<IEnumerable<TEntity>> GetListForIdsAsync(IEnumerable<Int64> id, QueryOptions? options = null);

        Task<int> UpdateForIdAsync(IDataDictionary data, Int64 id, QueryOptions? options = null);

        Task<int> DeleteForIdAsync(Int64 id, QueryOptions? options = null);
    }
}
