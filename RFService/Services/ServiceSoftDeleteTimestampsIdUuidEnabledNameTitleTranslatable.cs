using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabledNameTitle<TRepo, TEntity>(repo),
            IServiceTranslatable<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable
    {
        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceTranslatable<TEntity>)this).SanitizeIsTranslatableForAutoGet(data)
            );
    }
}
