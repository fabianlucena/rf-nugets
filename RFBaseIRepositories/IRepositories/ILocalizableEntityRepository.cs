using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface ILocalizableEntityRepository<T>
        : ITitledEntityRepository<T>
        where T : LocalizableEntity, new()
    {
    }
}