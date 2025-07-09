using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceLogIdUuidEnabled<TRepo, TEntity>(TRepo repo)
        : ServiceLogIdUuid<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntityLogIdUuidEnabled
    {
        public override GetOptions SanitizeGetOptions(GetOptions options)
        {
            if (!options.HasColumnFilter("IsEnabled")
                && !options.IncludeDisabled
            )
            {
                options = new GetOptions(options);
                options.AddFilter("IsEnabled", true);
            }

            return base.SanitizeGetOptions(options);
        }
    }
}
