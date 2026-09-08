using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIRepositories.IRepositories;

public interface INominableOwnedEntityRepository<T>
    : IOwnedEntityRepository<T>
    where T : NominableOwnedEntity, new()
{
    Task<IEnumerable<string>> GetNamesAsync(NominableEntityQueryOptions options);
}