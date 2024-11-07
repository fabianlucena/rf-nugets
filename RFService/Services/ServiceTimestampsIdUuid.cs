using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuid<Repo, Entity>(Repo repo)
        : ServiceTimestampsId<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuid
    {
        public override async Task<Entity> ValidateForCreationAsync(Entity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Uuid == null || data.Uuid == Guid.Empty)
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
