using RFService.Entities;
using RFService.Exceptions;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledName<Repo, Entity>(Repo repo)
        : ServiceTimestampsIdUuidEnabled<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuidEnabledName
    {
        public async Task<Entity> GetSingleForNameAsync(string name, GetOptions? options)
        {
            options ??= new GetOptions();
            options.Filters["Name"] = name;

            return await repo.GetSingleAsync(options);
        }

        public async Task<Entity?> GetSingleOrDefaultForNameAsync(string name, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Name"] = name;

            return await repo.GetSingleOrDefaultAsync(options);
        }

        public override GetOptions SanitizeForAutoGet(GetOptions options)
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

            return base.SanitizeForAutoGet(options);
        }

        public async Task<Int64> GetIdForNameAsync(string name, GetOptions? options = null, Func<string, Entity>? creator = null)
        {
            var item = await GetSingleOrDefaultForNameAsync(name, options);
            if (item == null)
            {
                if (creator != null)
                {
                    item = creator(name);
                }

                if (item == null)
                {
                    throw new NamedItemNotFoundException(name);
                }

                await CreateAsync(item);
            }

            return item.Id;
        }
    }
}
