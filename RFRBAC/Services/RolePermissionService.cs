using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class RolePermissionService(
        IRepo<RolePermission> repo
    ) : Service<IRepo<RolePermission>, RolePermission>(repo),
            IRolePermissionService
    {
        public async Task<IEnumerable<Int64>> GetPermissionsIdForRolesIdAsync(
            IEnumerable<Int64> rolesId,
            GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["RoleId"] = rolesId;
            var rolesPermissions = await GetListAsync(options);
            return rolesPermissions.Select(i => i.PermissionId);
        }
    }
}
