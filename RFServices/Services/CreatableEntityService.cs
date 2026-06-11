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
    : EntityService<T>(repository),
    ICreatableEntityService<T>
    where T : CreatableEntity, new()
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        if (entity.CreatedById == 0)
        {
            var userService = ServiceProvider.GetService<IUserService>()
                ?? throw new CreatedByIdMustBeSetForNewEntriesException();

            var createdById = await userService.GetCurrentOrSystemUserIdAsync();
            if (createdById <= 0)
                throw new CreatedByIdMustBeSetForNewEntriesException();

            entity.CreatedById = createdById;
        }

        entity.CreatedAt = DateTime.UtcNow;

        return entity;
    }
}
