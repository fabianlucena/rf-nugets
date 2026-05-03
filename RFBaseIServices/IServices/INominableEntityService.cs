using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIServices.IServices
{
    public interface INominableEntityService<T>
        : ICommonEntityService<T>
        where T : NominableEntity, new()
    {
        Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null);
        Task<long?> GetSingleOrDefaultIdByNameAsync(string name, NominableEntityQueryOptions? options = null);
    }
}