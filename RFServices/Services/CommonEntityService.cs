using RFBase.Libs;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class CommonEntityService<T>(
    ICommonEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : AuditableEntityService<T>(repository, serviceProvider),
    ICommonEntityService<T>
    where T : CommonEntity, new()
{
    public async Task<int> RestoreByIdAsync(long id)
    {
        var data = await ValidateForUpdate(new DataDictionary{
            { "deletedAt", null },
            { "deletedById", null  }
        });
        int success = await repository.UpdateByIdAsync(id, data);
        if (success == 0)
            throw new InvalidOperationException($"Failed to restore entity with ID {id}.");

        return success;
    }

    public async Task<int> RestoreByUuidAsync(Guid uuid)
    {
        var id = await GetSingleIdByUuidAsync(uuid);
        var data = await ValidateForUpdate(new DataDictionary{
            { "deletedAt", null },
            { "deletedById", null  }
        });
        int success = await repository.UpdateByIdAsync(id, data);
        if (success == 0)
            throw new InvalidOperationException($"Failed to restore entity with UUID {uuid}.");

        return success;
    }
}
