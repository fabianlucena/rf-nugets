using RFBaseEntities.Entities;

namespace RFBaseIServices.IServices
{
    public interface ICommonEntityService<T>
        : IAuditableEntityService<T>
        where T : CommonEntity, new()
    {
    }
}