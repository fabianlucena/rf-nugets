using RFBase.ILibs;
using RFRGOBAC.DTO;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IServices;

public interface ISystemUserService
{
    Task<SystemUser> CreateAsync(SystemUser user);
    Task<IEnumerable<SystemUser>> GetListAsync(SystemUserQueryOptions? options = null);
    Task<SystemUser?> GetSingleOrDefaultAsync(SystemUserQueryOptions? options = null);
    Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data, SystemUserQueryOptions? options = null);
    Task<int> DeleteByUuidAsync(Guid uuid, SystemUserQueryOptions? options = null);
    Task<int> RestoreByUuidAsync(Guid uuid, SystemUserQueryOptions? options = null);
}
