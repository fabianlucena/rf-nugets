using RFEntities.Entities;

namespace RFIServices.IServices;

public interface IALocalizableEntityService<T>
    : ILocalizableEntityService<T>
    where T : ALocalizableEntity, new()
{
}