using RFBase.Libs;
using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices;

public interface ICommonEntityService<T>
    : IAuditableEntityService<T>
    where T : CommonEntity, new()
{
    Task<int> DeleteByIdAsync(long id, CommonEntityQueryOptions? options = null);
    Task<int> DeleteByUuidAsync(Guid uuid, CommonEntityQueryOptions? options = null);
    Task<int> RestoreByIdAsync(long id, CommonEntityQueryOptions? options = null);
    Task<int> RestoreByUuidAsync(Guid uuid, CommonEntityQueryOptions? options = null);
}