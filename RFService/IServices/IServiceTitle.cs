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

        public async Task<TEntity> GetSingleForTitleAsync(string title, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Title", title);

            return await GetSingleAsync(options);
        }

        public async Task<TEntity?> GetSingleOrDefaultForTitleAsync(string title, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Title", title);

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
