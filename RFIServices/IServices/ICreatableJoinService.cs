using RFEntities.Entities;

namespace RFIServices.IServices
{
    public interface ICreatableJoinService<T>
        : IJoinService<T>
        where T : CreatableJoin, new()
    {
    }
}
