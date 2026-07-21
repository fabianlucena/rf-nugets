using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIRepositories.IRepositories
{
    public interface INominableEntityRepository<T>
        : ICommonEntityRepository<T>
        where T : NominableEntity, new()
    {
        Task<IEnumerable<string>> GetNamesAsync(NominableEntityQueryOptions options);
    }
}