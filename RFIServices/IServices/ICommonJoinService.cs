using RFEntities.Entities;

namespace RFIServices.IServices
{
    public interface ICommonJoinService<T>
        : ICreatableJoinService<T>
        where T : CommonJoin, new()
    {
    }
}