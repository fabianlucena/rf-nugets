using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class CommonJoinService<T>(
    ICommonJoinRepository<T> repository,
    IServiceProvider serviceProvider
)
    : CreatableJoinService<T>(repository, serviceProvider),
    ICommonJoinService<T>
    where T : CommonJoin, new()
{
}
