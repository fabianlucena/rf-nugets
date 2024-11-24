using RFRBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IUserRoleService
        : IService<UserRole>,
            IServiceTimestamps<UserRole>
    {
        Task<IEnumerable<Int64>> GetRolesIdAsync(GetOptions options);

        Task<IEnumerable<Int64>> GetRolesIdForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetRolesIdForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetAllRolesIdForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetAllRolesIdForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null);

        Task<IEnumerable<Role>> GetRolesForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Role>> GetAllRolesForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task UpdateRolesNameForUserNameAsync(string username, string[] rolesName);
    }
}
