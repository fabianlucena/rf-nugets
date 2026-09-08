using RFEntities.Entities;

namespace RFIServices.IServices;

public interface IANominableOwnedEntityService<T>
    : INominableOwnedEntityService<T>
    where T : ANominableOwnedEntity, new()
{
}