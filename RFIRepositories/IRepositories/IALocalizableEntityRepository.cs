using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface IALocalizableEntityRepository<T>
        : ILocalizableEntityRepository<T>
        where T : ALocalizableEntity, new()
    {
    }
}