using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class ALocalizableEntityService<T>(
    IALocalizableEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : LocalizableEntityService<T>(repository, serviceProvider),
    IALocalizableEntityService<T>
    where T : ALocalizableEntity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        return entity;
    }
}
