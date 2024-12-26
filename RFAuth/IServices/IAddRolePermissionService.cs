namespace RFAuth.IServices
{
    public interface IAddRolePermissionService
    {
        Task<bool> AddRolesPermissionsAsync(IDictionary<string, IEnumerable<string>> rolesPermissions);
    }
}
