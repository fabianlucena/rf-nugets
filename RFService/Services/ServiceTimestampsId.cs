using RFService.Exceptions;
using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsId<Repo, Entity>(Repo repo)
        : ServiceTimestamps<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsId
    {
        public override async Task<Entity> ValidateForCreationAsync(Entity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Id != 0)
            {
                throw new ForbidenIdForCreationException();
            }

            return data;
        }

        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("Id", out object? value))
            {
                options = new GetOptions(options);
                if (value != null
                    && (Int64)value > 0
                )
                {
                    options.Filters = new Dictionary<string, object?> { { "Id", value } };
                    return options;
                }
                else {
                    options.Filters.Remove("Id");
                }
            }

            return base.SanitizeForAutoGet(options);
        }

        public virtual Task<Entity> GetForIdAsync(Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Id"] = id;
            return GetSingleAsync(options);
        }

        public virtual Task<Entity?> GetSingleOrDefaultForIdAsync(Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Id"] = id;
            return GetSingleOrDefaultAsync(options);
        }

        public virtual Task<int> UpdateForIdAsync(object data, Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Id"] = id;
            return UpdateAsync(data, options);
        }

        public virtual Task<int> DeleteForIdAsync(Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Id"] = id;
            return DeleteAsync(options);
        }
    }
}