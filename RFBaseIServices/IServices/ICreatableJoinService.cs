using RFBaseEntities.Entities;

namespace RFBaseIServices.IServices
{
    public interface ICreatableJoinService<T>
        : IJoinService<T>
        where T : CreatableJoin, new()
    {
    }
}
