using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestamps<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDelete<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestamps
    {
        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            data.CreatedAt = DateTime.UtcNow;
            data.UpdatedAt = DateTime.UtcNow;

            return data;
        }

        public override async Task<IDictionary<string, object?>> ValidateForUpdateAsync(IDictionary<string, object?> data)
        {
            data = await base.ValidateForUpdateAsync(data);

            if (!data.ContainsKey("UpdatedAt"))
                data["UpdatedAt"] = DateTime.UtcNow;

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