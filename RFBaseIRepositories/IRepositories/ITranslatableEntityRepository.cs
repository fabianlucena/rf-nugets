using RFBaseEntities.Entities;

namespace RFBaseIRepositories.IRepositories
{
    public interface ITranslatableEntityRepository<T>
        : ICommonEntityRepository<T>
        where T : TranslatableEntity, new()
    {
    }
}