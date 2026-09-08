using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIRepositories.IRepositories;

public interface ICreatableWithNameEntityRepository<T>
    : ICreatableEntityRepository<T>
    where T : CreatableWithNameEntity, new()
{
    Task<IEnumerable<string>> GetNamesAsync(CreatableWithNameEntityQueryOptions options);
}