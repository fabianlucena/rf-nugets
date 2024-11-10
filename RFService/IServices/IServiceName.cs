using RFService.Exceptions;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceName<Entity>
        : IService<Entity>
        where Entity : class
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
                    options.Filters = new Dictionary<string, object?> { { "Name", value } };
                    return options;
                }
                else
                {
                    options.Filters.Remove("Name");
                }
            }

            return options;
        }

        public async Task<Entity> GetSingleForNameAsync(string name, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Name"] = name;

            return await GetSingleAsync(options);
        }

        public async Task<Entity?> GetSingleOrDefaultForNameAsync(string name, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Name"] = name;

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
