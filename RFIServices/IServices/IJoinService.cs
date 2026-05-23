using RFEntities.Entities;

namespace RFIServices.IServices
{
    public interface IJoinService<T>
        : IBaseService<T>
        where T : Join, new()
    {
    }
}
