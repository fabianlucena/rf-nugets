using RFRGBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRGBAC.IServices
{
    public interface IUserGroupService
        : IServiceTimestamps<UserGroup>
    {
        Task<IEnumerable<Int64>> GetGroupsIdForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetGroupsIdForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetAllGroupsIdForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Int64>> GetAllGroupsIdForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null);
    }
}
