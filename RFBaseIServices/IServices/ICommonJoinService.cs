using RFBaseEntities.Entities;

namespace RFBaseIServices.IServices
{
    public interface ICommonJoinService<T>
        : ICreatableJoinService<T>
        where T : CommonJoin, new()
    {
    }
}