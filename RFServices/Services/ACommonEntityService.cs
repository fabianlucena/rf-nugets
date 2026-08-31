using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class ACommonEntityService<T>(
    IACommonEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : CommonEntityService<T>(repository, serviceProvider),
    IACommonEntityService<T>
    where T : ACommonEntity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        return entity;
    }
}
