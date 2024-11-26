using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestamps<Repo, Entity>(Repo repo)
        : Service<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestamps
    {
        public override async Task<Entity> ValidateForCreationAsync(Entity data)
        {
            data = await base.ValidateForCreationAsync(data);

            data.CreatedAt = DateTime.UtcNow;
            data.UpdatedAt = DateTime.UtcNow;

            return data;
        }

        public override async Task<IDictionary<string, object?>> ValidateForUpdateAsync(IDictionary<string, object?> data)
        {
            data = await base.ValidateForUpdateAsync(data);

            if (!data.ContainsKey("UpdatedAt")
                && !data.ContainsKey("DeletedAt")
            )
            {
                data["UpdatedAt"] = DateTime.UtcNow;
            }

            return data;
        }

        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            var newOptions = false;
            if (options.Filters.TryGetValue("CreatedAt", out object? value))
            {
                if (value == null
                    || (DateTime)value == DateTime.MinValue
                )
                {
                    options = new GetOptions(options);
                    newOptions = true;
                    options.Filters.Remove("CreatedAt");
                }
            }

            if (options.Filters.TryGetValue("UpdatedAt", out value))
            {
                if (value == null
                    || (DateTime)value == DateTime.MinValue
                )
                {
                    if (!newOptions)
                    {
                        options = new GetOptions(options);
                    }

                    options.Filters.Remove("UpdatedAt");
                }
            }

            return base.SanitizeForAutoGet(options);
        }
    }
}