using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface ILocalizableEntityRepository<T>
        : ITitledEntityRepository<T>
        where T : LocalizableEntity, new()
    {
    }
}