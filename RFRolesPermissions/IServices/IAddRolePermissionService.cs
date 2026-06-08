namespace RFRolesPermissions.IServices;

public interface IAddRolePermissionService
{
    Task<int> AddRolesPermissionsAsync(Dictionary<string, IEnumerable<string>> rolesPermissions);
}
