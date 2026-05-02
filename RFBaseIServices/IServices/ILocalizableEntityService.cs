using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIServices.IServices
{
    public interface ILocalizableEntityService<T>
        : ITitledEntityService<T>
        where T : LocalizableEntity, new()
    {
    }
}