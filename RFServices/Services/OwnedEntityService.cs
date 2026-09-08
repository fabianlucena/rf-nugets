using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class OwnedEntityService<T>(
    IOwnedEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : CommonEntityService<T>(repository, serviceProvider),
    IOwnedEntityService<T>
    where T : OwnedEntity, new()
{
}
