using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class TranslatableEntityService<T>(
    ITranslatableEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : CommonEntityService<T>(repository, serviceProvider),
    ITranslatableEntityService<T>
    where T : TranslatableEntity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        return entity;
    }
}
