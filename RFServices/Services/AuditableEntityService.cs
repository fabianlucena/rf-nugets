using RFBase.ILibs;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFIServices.QueryOptions;
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
            entity.UpdatedById = await GetCurrentUserIdAsync();
            if (entity.UpdatedById <= 0)
                throw new CreatedByIdMustBeSetForNewEntriesException();
        }

        entity.UpdatedAt = DateTime.UtcNow;

        return entity;
    }

    public override async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, BaseQueryOptions options)
    {
        data = await base.ValidateForUpdateAsync(data, options);

        if (!data.TryGetValue("UpdatedById", out object? value) || value is null || (long)value <= 0)
        {
            var updatedById = await GetCurrentUserIdAsync();
            if (updatedById <= 0)
                throw new UpdatedByIdMustBeSetForAuditableEntriesException();

            data["UpdatedById"] = updatedById;
        }

        data["UpdatedAt"] = DateTime.UtcNow;

        return data;
    }
}
