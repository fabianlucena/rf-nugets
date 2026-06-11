using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class CommonJoinService<T>(ICommonJoinRepository<T> repository)
    : CreatableJoinService<T>(repository),
    ICommonJoinService<T>
    where T : CommonJoin, new()
{
}
