using RFEntities.Entities;

namespace RFIServices.IServices
{
    public interface ICommonEntityService<T>
        : IAuditableEntityService<T>
        where T : CommonEntity, new()
    {
    }
}