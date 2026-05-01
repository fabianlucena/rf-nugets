using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIServices.IServices
{
    public interface INominableEntityService<T>
        : ICommonEntityService<T>
        where T : NominableEntity, new()
    {
        Task<T?> GetSingleOrDefaultByNameAsync(string name, BaseQueryOptions? options = null);
        Task<long?> GetSingleOrDefaultIdByNameAsync(string name, BaseQueryOptions? options = null);
    }
}