using RFEntities.Entities;

namespace RFIServices.IServices;

public interface IACommonEntityService<T>
    : ICommonEntityService<T>
    where T : ACommonEntity, new()
{
}