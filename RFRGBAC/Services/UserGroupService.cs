using RFRGBAC.Entities;
using RFRGBAC.IServices;
using RFService.IRepo;
using RFService.Operator;
using RFService.Repo;
using RFService.Services;

namespace RFRGBAC.Services
{
    public class UserGroupService(
        IRepo<UserGroup> repo
    ) : ServiceTimestamps<IRepo<UserGroup>, UserGroup>(repo),
            IUserGroupService
    {
        public async Task<IEnumerable<long>> GetGroupsIdForUsersIdAsync(
            IEnumerable<Int64> usersId,
            GetOptions? options = null
        )
        {
            options ??= new GetOptions();
            options.Filters["UserId"] = usersId;
            var usersGroups = await GetListAsync(options);
            return usersGroups.Select(i => i.GroupId);
        }

        public async Task<IEnumerable<long>> GetAllGroupsIdForUsersIdAsync(
            IEnumerable<Int64> usersId,
            GetOptions? options = null
        )
        {
            var allGroupsId = usersId.ToList();

            options ??= new GetOptions();
            options.Filters["GroupId"] = Op.DistinctTo(allGroupsId);
            var newGroupsId = await GetGroupsIdForUsersIdAsync(usersId, options);
            while (newGroupsId.Any())
            {
                allGroupsId.AddRange(newGroupsId);
                options.Filters["ParentId"] = Op.DistinctTo(allGroupsId);
                newGroupsId = await GetGroupsIdForUsersIdAsync(newGroupsId, options);
            }

            return allGroupsId;
        }
    }
}
