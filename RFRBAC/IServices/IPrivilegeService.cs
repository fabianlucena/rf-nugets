namespace RFRBAC.IServices
{
    public interface IPrivilegeService
    {
        Task<bool> UserIdHasAnyRoleAsync(Int64 userId, params string[] checkingRoles);

        Task<bool> UserIdHasAnyPermissionAsync(Int64 userId, params string[] checkingPermissions);

        Task<bool> HasAnyRoleAsync(params string[] checkingRoles);

        Task<bool> HasAnyPermissionAsync(params string[] checkingPermissions);

        Task<bool> HasRoleAsync(string checkingRole)
            => HasAnyRoleAsync([checkingRole]);

        Task<bool> HasPermissionAsync(string checkingPermission)
            => HasAnyPermissionAsync([checkingPermission]);
    }
}
