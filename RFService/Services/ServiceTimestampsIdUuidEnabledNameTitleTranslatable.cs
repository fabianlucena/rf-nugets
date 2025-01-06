using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;

namespace RFService.Services
{
    public abstract class ServiceTimestampsIdUuidEnabledNameTitleTranslatable<TRepo, TEntity>(TRepo repo)
        : ServiceTimestampsIdUuidEnabledNameTitle<TRepo, TEntity>(repo),
            IServiceTranslatable<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsIdUuidEnabledNameTitleTranslatable
    {
        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceTranslatable<TEntity>)this).SanitizeIsTranslatableForAutoGet(data)
            );
    }
}
