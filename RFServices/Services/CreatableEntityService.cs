using Microsoft.Extensions.DependencyInjection;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFServices.Exceptions;

namespace RFServices.Services;

public class CreatableEntityService<T>(
    ICreatableEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : EntityService<T>(repository, serviceProvider),
    ICreatableEntityService<T>
    where T : CreatableEntity, new()
{
    public IUserService UserService => ServiceProvider.GetRequiredService<IUserService>();

    protected long catchedCurrentUserId = 0;
    public virtual async Task<long> GetCurrentUserId()
    {
        if (catchedCurrentUserId <= 0)
            catchedCurrentUserId = await UserService.GetCurrentUserIdAsync();

        return catchedCurrentUserId;
    }

    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        if (entity.CreatedById <= 0)
        {
            entity.CreatedById = await GetCurrentUserId();
            if (entity.CreatedById <= 0)
                throw new CreatedByIdMustBeSetForNewEntriesException();
        }

        entity.CreatedAt = DateTime.UtcNow;

        return entity;
    }
}
