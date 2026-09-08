using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class ANominableOwnedEntityService<T>(
    IANominableOwnedEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : NominableOwnedEntityService<T>(repository, serviceProvider),
    IANominableOwnedEntityService<T>
    where T : ANominableOwnedEntity, new ()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        return entity;
    }
}
