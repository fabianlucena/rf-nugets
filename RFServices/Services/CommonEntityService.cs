using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class CommonEntityService<T>(
    ICommonEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : AuditableEntityService<T>(repository, serviceProvider),
    ICommonEntityService<T>
    where T : CommonEntity, new()
{}
