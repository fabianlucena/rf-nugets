using RFAuth.IServices;
using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class RolePermissionService(
        IRepo<RolePermission> repo,
        IRoleService roleService,
        IPermissionService permissionService,
        IUserRoleService userRoleService
    ) : ServiceTimestamps<IRepo<RolePermission>, RolePermission>(repo),
        IRolePermissionService,
        IAddRolePermissionService
    {
        public async Task<IEnumerable<Int64>> GetPermissionsIdForRolesIdAsync(
            IEnumerable<Int64> rolesId,
            QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("RoleId", rolesId);
            var rolesPermissions = await GetListAsync(options);
            return rolesPermissions.Select(i => i.PermissionId);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsForRolesIdAsync(
            IEnumerable<Int64> rolesId,
            QueryOptions? options = null)
        {
            var permissionsId = await GetPermissionsIdForRolesIdAsync(rolesId, options);
            return await permissionService.GetListForIdsAsync(permissionsId);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsForRoleIdListAsync(
            IEnumerable<Int64> rolesId,
            QueryOptions? options = null
        )
        {
            var permissionsId = await GetPermissionsIdForRolesIdAsync(rolesId);

            options ??= new QueryOptions();
            options.AddFilter("Id", permissionsId);

            return await permissionService.GetListAsync(options);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsForUserIdAsync(
            Int64 userId,
            QueryOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUserIdAsync(userId);
            return await GetPermissionsForRoleIdListAsync(allRolesId);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsForUsersIdAsync(
            IEnumerable<Int64> usersId,
            QueryOptions? options = null
        )
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUsersIdAsync(usersId);
            return await GetPermissionsForRoleIdListAsync(allRolesId);
        }

        public async Task<bool> AddRolesPermissionsAsync(IDictionary<string, IEnumerable<string>> rolesPermissions)
        {
            foreach (var rolePermissions in rolesPermissions)
            {
                var requiredPermissionsName = rolePermissions.Value;
                var roleId = await roleService.GetSingleIdForNameAsync(
                    rolePermissions.Key,
                    creator: name => new Role
                    {
                        Name = name,
                        Title = name,
                    }
                );
                var requiredPermissions = await permissionService.GetListForNamesAsync(requiredPermissionsName);
                if (requiredPermissions.Count() != requiredPermissionsName.Count())
                {
                    var currentPermissionsName = requiredPermissions.Select(x => x.Name);
                    var newPermissionsName = requiredPermissionsName.Except(currentPermissionsName);
                    foreach (var permissionName in newPermissionsName)
                    {
                        await permissionService.CreateAsync(new Permission {
                            Name = permissionName,
                            Title = permissionName
                        });
                    }

                    requiredPermissions = await permissionService.GetListForNamesAsync(requiredPermissionsName);
                }

                var requiredPermissionsId = requiredPermissions.Select(x => x.Id);

                var currentPermissionsId = (await GetListAsync(new QueryOptions
                    {
                        Filters = {
                            { "RoleId", roleId },
                            { "PermissionId", requiredPermissionsId }
                        }
                    }))
                    .Select(i => i.PermissionId);
                var newPermissionsId = requiredPermissionsId.Except(currentPermissionsId);

                if (newPermissionsId.Any())
                {
                    var rolePermission = new RolePermission { RoleId = roleId };
                    foreach (var newPermissionId in newPermissionsId)
                    {
                        rolePermission.PermissionId = newPermissionId;
                        await CreateAsync(rolePermission);
                    }
                }
            }

            return true;
        }
    }
}
