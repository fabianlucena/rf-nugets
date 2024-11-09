using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDelete<Repo, Entity>(Repo repo)
        : Service<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntitySoftDelete
    {
        public override GetOptions SanitizeGetOptions(GetOptions options)
        {
            if (!options.Filters.ContainsKey("DeletedAt"))
                options.Filters["DeletedAt"] = null;

            return base.SanitizeGetOptions(options);
        }

        public override Task<int> DeleteAsync(GetOptions options)
        {
            return UpdateAsync(new { DeletedAt = DateTime.UtcNow }, options);
        }
    }
}