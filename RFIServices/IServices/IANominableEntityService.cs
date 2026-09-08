using RFEntities.Entities;

namespace RFIServices.IServices;

public interface IANominableEntityService<T>
    : INominableEntityService<T>
    where T : ANominableEntity, new()
{
}