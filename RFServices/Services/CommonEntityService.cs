using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services
{
    public class CommonEntityService<T>(ICommonEntityRepository<T> repository)
        : AuditableEntityService<T>(repository),
        ICommonEntityService<T>
        where T : CommonEntity, new()
    {
    }
}
