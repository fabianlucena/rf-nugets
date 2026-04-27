using RFBaseEntities.Entities;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

namespace RFBaseServices.Services
{
    public class CommonEntityService<T>(ICommonEntityRepository<T> repository)
        : AuditableEntityService<T>(repository),
        ICommonEntityService<T>
        where T : CommonEntity, new()
    {
    }
}
