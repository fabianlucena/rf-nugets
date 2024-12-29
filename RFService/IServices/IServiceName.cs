using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceName<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        public GetOptions SanitizeNameForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("Name", out object? value))
            {
                options = new GetOptions(options);
                if (value != null
                    && !string.IsNullOrEmpty((string)value)
                )
                {
                    options.Filters = new DataDictionary { { "Name", value } };
                    return options;
                }
                else
                {
                    options.Filters.Remove("Name");
                }
            }

            return options;
        }

        Task<TEntity> GetSingleForNameAsync(string name, GetOptions? options = null);

        Task<TEntity?> GetSingleOrDefaultForNameAsync(string name, GetOptions? options = null);
    }
}
