using RFBaseEntities.Entities;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

namespace RFBaseServices.Services
{
    public class ImmutableEntityService<T>(IImmutableEntityRepository<T> repository)
        : CreatableEntityService<T>(repository),
        IImmutableEntityService<T>
        where T : ImmutableEntity, new()
    {
    }
}
