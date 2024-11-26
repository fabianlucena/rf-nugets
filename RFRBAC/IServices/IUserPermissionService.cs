using RFRBAC.Entities;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IUserPermissionService
    {
        Task<IEnumerable<Permission>> GetAllPermissionsForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Permission>> GetAllPermissionsForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null);

        Task<bool> UserIdHasAnyPermissionAsync(Int64 userId, params string[] checkingPermissions);
    }
}
