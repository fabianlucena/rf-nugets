using RFService.Entities;
using RFService.Exceptions;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledTitle<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabled<TRepo, TEntity>(repo),
            IServiceUniqueTitle<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsIdUuidEnabledTitle
    {
        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceTitle<TEntity>)this).SanitizeTitleForAutoGet(data)
            );

        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            if (string.IsNullOrEmpty(data.Title))
                throw new TitleCannotBeNullOrEmptyException();

            var entity = await GetSingleOrDefaultForTitleAsync(data.Title);
            if (entity != null)
                throw new ARowWithTheNameAlreadyExistsException(data.Title);

            data = await base.ValidateForCreationAsync(data);

            return data;
        }

        public async Task<TEntity?> GetSingleOrDefaultForTitleAsync(string title, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Title", title);

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
