using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class ImmutableEntityService<T>(
    IImmutableEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : CreatableEntityService<T>(repository, serviceProvider),
    IImmutableEntityService<T>
    where T : ImmutableEntity, new()
{}
