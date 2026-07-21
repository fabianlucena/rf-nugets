using RFEntities.Entities;

namespace RFIRepositories.IRepositories
{
    public interface ITranslatableEntityRepository<T>
        : ICommonEntityRepository<T>
        where T : TranslatableEntity, new()
    {
    }
}