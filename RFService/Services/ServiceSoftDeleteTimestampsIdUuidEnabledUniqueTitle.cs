using RFService.Entities;
using RFService.Exceptions;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsIdUuidEnabledUniqueTitle<Repo, Entity>(Repo repo)
        : ServiceSoftDeleteTimestampsIdUuidEnabled<Repo, Entity>(repo),
            IServiceUniqueTitle<Entity>
        where Repo : IRepo<Entity>
        where Entity : EntitySoftDeleteTimestampsIdUuidEnabledUniqueTitle
    {
        public override async Task<Entity> ValidateForCreationAsync(Entity data)
        {
            if (string.IsNullOrEmpty(data.Title))
                throw new TitleCannotBeNullOrEmptyException();

            var entity = await GetSingleOrDefaultForTitleAsync(data.Title);
            if (entity != null)
                throw new ARowWithTheNameAlreadyExistsException(data.Title);

            data = await base.ValidateForCreationAsync(data);

            return data;
        }

        public async Task<Entity?> GetSingleOrDefaultForTitleAsync(string title, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Title"] = title;

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
