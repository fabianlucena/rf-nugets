using RFEntities.Entities;

namespace RFIServices.IServices
{
    public interface INoIdEntityService<T>
        : IBaseService<T>
        where T : NoIdEntity, new()
    {
    }
}