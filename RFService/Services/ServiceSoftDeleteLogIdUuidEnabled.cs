using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteLogIdUuidEnabled<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteLogIdUuid<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteLogIdUuidEnabled
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
