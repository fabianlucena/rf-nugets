using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;
using RFUserGroups.Entities;
using RFUserGroups.IRepositories;
using RFUserGroups.QueryOptions;

namespace RFUserGroupsEF.Repositories;

[RegisterService]
public class UserGroupRepository(DbContext context)
    : CommonJoinRepository<UserGroup>(context),
    IUserGroupRepository
{
    public override IQueryable<UserGroup> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        queryable = queryable.OrderBy(ug => ug.UserId)
            .ThenBy(ug => ug.GroupId);

        if (options is UserGroupQueryOptions userGroupOptions)
        {
            if (userGroupOptions.IncludeUser)
                queryable = queryable.Include(ug => ug.User);

            if (userGroupOptions.IncludeGroup)
                queryable = queryable.Include(ug => ug.Group);

            if (userGroupOptions.GroupId is not null)
                queryable = queryable.Where(ug => ug.GroupId == userGroupOptions.GroupId);

            if (userGroupOptions.UserId is not null)
                queryable = queryable.Where(ug => ug.UserId == userGroupOptions.UserId);

            if (userGroupOptions.UsersId is not null)
                queryable = queryable.Where(ug => userGroupOptions.UsersId.Contains(ug.UserId));
        }

        return queryable;
    }

    public async Task<IEnumerable<long>> GetAllGroupsIdByUsersIdAsync(IEnumerable<long> usersId, UserGroupQueryOptions? options = null)
    {
        var set = GetDBSet(options);
        var result = usersId.ToList();
        var lastResult = usersId.ToList();
        do
        {
            lastResult = await set
                .Where(r => lastResult.Contains(r.UserId)
                    && !result.Contains(r.GroupId)
                )
                .Select(r => r.GroupId)
                .ToListAsync();

            result.AddRange(lastResult);
        } while (lastResult.Count != 0);

        return result;
    }
}
