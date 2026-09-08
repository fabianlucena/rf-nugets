using RFEntities.Entities;

namespace RFIServices.IServices;

public interface IOwnedEntityService<T>
    : ICommonEntityService<T>
    where T : OwnedEntity, new()
{
}