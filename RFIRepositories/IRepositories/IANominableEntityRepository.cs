using RFEntities.Entities;

namespace RFIRepositories.IRepositories;

public interface IANominableEntityRepository<T>
    : INominableEntityRepository<T>
    where T : ANominableEntity, new()
{
}