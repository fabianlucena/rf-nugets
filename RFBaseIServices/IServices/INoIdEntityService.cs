using RFBaseEntities.Entities;

namespace RFBaseIServices.IServices
{
    public interface INoIdEntityService<T>
        : IBaseService<T>
        where T : NoIdEntity, new()
    {
    }
}