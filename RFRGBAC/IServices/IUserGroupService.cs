using RFRGBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRGBAC.IServices
{
    public interface IUserGroupService
        : IServiceTimestamps<UserGroup>
    {
        Task<IEnumerable<long>> GetAllGroupsIdForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<long>> GetAllGroupsIdForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null);
    }
}
