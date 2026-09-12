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

    public virtual async Task<long> GetCurrentUserIdAsync()
        => await UserService.GetCurrentUserIdAsync();

    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        if (entity.CreatedById <= 0)
        {
            entity.CreatedById = await GetCurrentUserIdAsync();
            if (entity.CreatedById <= 0)
                throw new CreatedByIdMustBeSetForNewEntriesException();
        }

        entity.CreatedAt = DateTime.UtcNow;

        return entity;
    }
}
