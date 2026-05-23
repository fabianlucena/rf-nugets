using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services
{
    public class ImmutableEntityService<T>(IImmutableEntityRepository<T> repository)
        : CreatableEntityService<T>(repository),
        IImmutableEntityService<T>
        where T : ImmutableEntity, new()
    {
    }
}
