using RFOperators;
using RFRGBAC.Entities;
using RFRGBAC.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFRGBAC.Services
{
    public class UserGroupService(
        IRepo<UserGroup> repo
    ) : ServiceTimestamps<IRepo<UserGroup>, UserGroup>(repo),
            IUserGroupService
    {
        public async Task<IEnumerable<Int64>> GetGroupsIdForUserIdAsync(
            Int64 userId,
            GetOptions? options = null
        )
        {
            options ??= new GetOptions();
            options.AddFilter("UserId", userId);
            var usersGroups = await GetListAsync(options);
            return usersGroups.Select(i => i.GroupId);
        }

        public async Task<IEnumerable<Int64>> GetGroupsIdForUsersIdAsync(
            IEnumerable<Int64> usersId,
            GetOptions? options = null
        )
        {
            options ??= new GetOptions();
            options.AddFilter("UserId", usersId);
            var usersGroups = await GetListAsync(options);
            return usersGroups.Select(i => i.GroupId);
        }

        public async Task<IEnumerable<Int64>> GetAllGroupsIdForUsersIdAsync(
            IEnumerable<Int64> usersId,
            GetOptions? options = null
        )
        {
            var allGroupsId = usersId.ToList();

            options ??= new GetOptions();
            options.AddFilter(Op.NE("GroupId", allGroupsId));
            var newGroupsId = await GetGroupsIdForUsersIdAsync(usersId, options);
            while (newGroupsId.Any())
            {
                allGroupsId.AddRange(newGroupsId);
                options.AddFilter(Op.NE("ParentId", allGroupsId));
                newGroupsId = await GetGroupsIdForUsersIdAsync(newGroupsId, options);
            }

            return allGroupsId;
        }

        public async Task<IEnumerable<Int64>> GetAllGroupsIdForUserIdAsync(
            Int64 userId,
            GetOptions? options = null
        )
        {
            var allGroupsId = new List<Int64> { userId };

            options ??= new GetOptions();
            options.AddFilter(Op.NE("GroupId", allGroupsId));
            var newGroupsId = await GetGroupsIdForUserIdAsync(userId, options);
            while (newGroupsId.Any())
            {
                allGroupsId.AddRange(newGroupsId);
                options.AddFilter(Op.NE("ParentId", allGroupsId));
                newGroupsId = await GetGroupsIdForUsersIdAsync(newGroupsId, options);
            }

            return allGroupsId;
        }
    }
}
