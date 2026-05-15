using RFBaseIServices.IServices;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIServices.IServices
{
    public interface IPermissionXRoleService : ICommonJoinService<PermissionXRole>
    {                       
        Task<IEnumerable<long>> GetPermissionIdsByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null);
        Task<IEnumerable<string>> GetPermissionNamesByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null);
    }
}
