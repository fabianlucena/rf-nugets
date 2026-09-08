using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class ANominableEntityService<T>(
    IANominableEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : NominableEntityService<T>(repository, serviceProvider),
    IANominableEntityService<T>
    where T : ANominableEntity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        return entity;
    }
}
