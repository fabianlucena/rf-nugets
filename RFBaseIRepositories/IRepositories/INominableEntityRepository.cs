using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIRepositories.IRepositories
{
    public interface INominableEntityRepository<T>
        : ICommonEntityRepository<T>
        where T : NominableEntity, new()
    {
        Task<T?> GetSingleOrDefaultByNameAsync(string name, BaseQueryOptions? options = null);
        Task<long?> GetSingleOrDefaultIdByNameAsync(string name, BaseQueryOptions? options = null);
    }
}