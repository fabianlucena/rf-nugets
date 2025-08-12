using RFService.ILibs;
using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceTitle<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        IDataDictionary SanitizeTitleForAutoGet(IDataDictionary data)
        {
            if (data.TryGetValue("Title", out object? value))
            {
                if (string.IsNullOrEmpty((string?)value))
                {
                    data = new DataDictionary(data);
                    data.Remove("Title");
                }
            }

            return data;
        }

        public async Task<TEntity> GetSingleForTitleAsync(string title, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Title", title);

            return await GetSingleAsync(options);
        }

        public async Task<TEntity?> GetSingleOrDefaultForTitleAsync(string title, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Title", title);

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
