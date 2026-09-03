using Microsoft.Extensions.DependencyInjection;
using RFBase.ILibs;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFServices.Exceptions;

namespace RFServices.Services;

public class NoIdEntityService<T>(
    INoIdEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : BaseService<T>(repository, serviceProvider),
    INoIdEntityService<T>
    where T : NoIdEntity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);


        if (entity.CreatedById == 0)
        {
            var userService = ServiceProvider.GetService<IUserService>()
                ?? throw new CreatedByIdMustBeSetForNewEntriesException();

            var createdById = await userService.GetCurrentUserIdAsync();
            if (createdById <= 0)
                throw new CreatedByIdMustBeSetForNewEntriesException();

            entity.CreatedById = createdById;
        }

        entity.CreatedAt = DateTime.UtcNow;

        if (entity.UpdatedById <= 0)
        {
            var userService = ServiceProvider.GetService<IUserService>()
                ?? throw new CreatedByIdMustBeSetForNewEntriesException();

            entity.UpdatedById = await userService.GetCurrentUserIdAsync();
            if (entity.UpdatedById <= 0)
                throw new CreatedByIdMustBeSetForNewEntriesException();
        }

        entity.UpdatedAt = DateTime.UtcNow;

        return entity;
    }

    public override async Task<IDataDictionary> ValidateForUpdate(IDataDictionary data)
    {
        data = await base.ValidateForUpdate(data);

        if (!data.TryGetValue("UpdatedById", out object? value) || value is null || (long)value <= 0)
        {
            var userService = ServiceProvider.GetService<IUserService>()
                ?? throw new CreatedByIdMustBeSetForNewEntriesException();

            var updatedById = await userService.GetCurrentUserIdAsync();
            if (updatedById <= 0)
                throw new UpdatedByIdMustBeSetForAuditableEntriesException();

            data["UpdatedById"] = updatedById;
        }

        data["UpdatedAt"] = DateTime.UtcNow;

        return data;
    }
}
