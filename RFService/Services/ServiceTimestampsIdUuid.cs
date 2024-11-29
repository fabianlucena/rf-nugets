using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuid<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsId<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuid
    {
        public Guid GetUuid(TEntity item) => item.Uuid;

        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Uuid == Guid.Empty)
            {
                data.Uuid = Guid.NewGuid();
            }

            return data;
        }

        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("Uuid", out object? value))
            {
                options = new GetOptions(options);
                if (value != null
                    && (Guid)value != Guid.Empty
                )
                {
                    options.Filters = new Dictionary<string, object?> { { "Uuid", value } };
                    return options;
                }
                else
                {
                    options.Filters.Remove("Uuid");
                }
            }

            return base.SanitizeForAutoGet(options);
        }
    }
}
