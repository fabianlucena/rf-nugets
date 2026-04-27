using RFBaseEntities.Entities;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

namespace RFBaseServices.Services
{
    public class CommonJoinService<T>(ICommonJoinRepository<T> repository)
        : CreatableJoinService<T>(repository),
        ICommonJoinService<T>
        where T : CommonJoin, new()
    {
    }
}
