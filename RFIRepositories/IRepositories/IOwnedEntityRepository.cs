using RFEntities.Entities;

namespace RFIRepositories.IRepositories;

public interface IOwnedEntityRepository<T>
    : ICommonEntityRepository<T>
    where T : OwnedEntity, new()
{
}