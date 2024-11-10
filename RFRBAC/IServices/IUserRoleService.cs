using RFRBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IUserRoleService
        : IServiceTimestamps<UserRole>
    {
        Task<IEnumerable<Int64>> GetRolesIdAsync(GetOptions options);

        Task<IEnumerable<Int64>> GetRolesIdForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetAllRolesIdForUserIdAsync(Int64 userId, GetOptions? options = null);
    }
}
