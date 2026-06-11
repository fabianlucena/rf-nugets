using Microsoft.Extensions.DependencyInjection;
using RFBase.ILibs;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFServices.Exceptions;

namespace RFServices.Services;

public class AuditableEntityService<T>(
    IAuditableEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : CreatableEntityService<T>(repository, serviceProvider),
    IAuditableEntityService<T>
    where T : AuditableEntity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        if (entity.UpdatedById <= 0)
        {
            var userService = ServiceProvider.GetService<IUserService>()
                ?? throw new UpdatedByIdMustBeSetForAuditableEntriesException();

            var updatedById = await userService.GetCurrentOrSystemUserIdAsync();
            if (updatedById <= 0)
                throw new UpdatedByIdMustBeSetForAuditableEntriesException();

            entity.UpdatedById = updatedById;
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
                ?? throw new UpdatedByIdMustBeSetForAuditableEntriesException();

            var updatedById = await userService.GetCurrentOrSystemUserIdAsync();
            if (updatedById <= 0)
                throw new UpdatedByIdMustBeSetForAuditableEntriesException();

            data["UpdatedById"] = updatedById;
        }

        data["UpdatedAt"] = DateTime.UtcNow;

        return data;
    }
}
