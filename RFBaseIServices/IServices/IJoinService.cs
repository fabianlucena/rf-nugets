using RFBaseEntities.Entities;

namespace RFBaseIServices.IServices
{
    public interface IJoinService<T>
        : IBaseService<T>
        where T : Join, new()
    {
    }
}
