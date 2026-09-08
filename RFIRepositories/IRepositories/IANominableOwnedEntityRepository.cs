using RFEntities.Entities;

namespace RFIRepositories.IRepositories;

public interface IANominableOwnedEntityRepository<T>
    : INominableOwnedEntityRepository<T>
    where T : ANominableOwnedEntity, new()
{
}