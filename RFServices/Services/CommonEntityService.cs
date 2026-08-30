using RFBase.Libs;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFIServices.QueryOptions;

namespace RFServices.Services;

public class CommonEntityService<T>(
    ICommonEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : AuditableEntityService<T>(repository, serviceProvider),
    ICommonEntityService<T>
    where T : CommonEntity, new()
{
    public virtual async Task<int> DeleteByIdAsync(long id, CommonEntityQueryOptions? options = null)
    {
        var data = await ValidateForUpdate(new DataDictionary{
            { "DeletedAt", DateTime.UtcNow },
            { "DeletedById", await GetCurrentUserId() }
        });
        int success = await repository.UpdateByIdAsync(id, data, options);
        if (success == 0)
            throw new InvalidOperationException($"Failed to delete entity with ID {id}.");

        return success;
    }

    public virtual async Task<int> DeleteByUuidAsync(Guid uuid, CommonEntityQueryOptions? options = null)
    {
        var id = await GetSingleIdByUuidAsync(uuid, options);
        var data = await ValidateForUpdate(new DataDictionary{
            { "DeletedAt", DateTime.UtcNow },
            { "DeletedById", await GetCurrentUserId() }
        });
        int success = await repository.UpdateByIdAsync(id, data, options);
        if (success == 0)
            throw new InvalidOperationException($"Failed to delete entity with UUID {uuid}.");

        return success;
    }

    public async Task<int> RestoreByIdAsync(long id, CommonEntityQueryOptions? options = null)
    {
        var data = await ValidateForUpdate(new DataDictionary{
            { "DeletedAt", null },
            { "DeletedById", null }
        });
        int success = await repository.UpdateByIdAsync(id, data, options);
        if (success == 0)
            throw new InvalidOperationException($"Failed to restore entity with ID {id}.");

        return success;
    }

    public async Task<int> RestoreByUuidAsync(Guid uuid, CommonEntityQueryOptions? options = null)
    {
        var id = await GetSingleIdByUuidAsync(uuid, options);
        var data = await ValidateForUpdate(new DataDictionary{
            { "DeletedAt", null },
            { "DeletedById", null }
        });
        int success = await repository.UpdateByIdAsync(id, data, options);
        if (success == 0)
            throw new InvalidOperationException($"Failed to restore entity with UUID {uuid}.");

        return success;
    }
}
