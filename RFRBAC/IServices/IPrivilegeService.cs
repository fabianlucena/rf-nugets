namespace RFRBAC.IServices
{
    public interface IPrivilegeService
    {
        Task<bool> UserIdHasRoleAsync(Int64 userId, params string[] checkingRoles);

        Task<bool> HasRoleAsync(params string[] checkingRoles);
    }
}
