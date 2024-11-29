using RFService.Entities;
using RFService.Exceptions;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledUniqueTitle<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabled<TRepo, TEntity>(repo),
            IServiceUniqueTitle<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsIdUuidEnabledUniqueTitle
    {
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

        public async Task<TEntity?> GetSingleOrDefaultForTitleAsync(string title, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Title"] = title;

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
