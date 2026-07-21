using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class TitledEntityService<T>(
    ITitledEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : NominableEntityService<T>(repository, serviceProvider),
    ITitledEntityService<T>
    where T : TitledEntity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        return entity;
    }
}
