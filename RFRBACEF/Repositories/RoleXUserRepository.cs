using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.QueryOptions;
using RFRegisterService.Attributes;

namespace RFRBACEF.Repositories;

[RegisterService]
public class RoleXUserRepository(DbContext context)
    : CommonJoinRepository<RoleXUser>(context),
    IRoleXUserRepository
{
    public override IQueryable<RoleXUser> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        queryable = queryable.OrderBy(ru => ru.RoleId);

        if (options is RoleXUserQueryOptions roleXUserOptions)
        {
            if (roleXUserOptions.IncludeRole)
                queryable = queryable.Include(ru => ru.Role);

            if (roleXUserOptions.IncludeUser)
                queryable = queryable.Include(ru => ru.User);

            if (roleXUserOptions.RoleId.HasValue)
                queryable = queryable.Where(ru => ru.RoleId == roleXUserOptions.RoleId.Value);

            if (roleXUserOptions.RoleIds is not null)
                queryable = queryable.Where(ru => roleXUserOptions.RoleIds.Contains(ru.RoleId));

            if (roleXUserOptions.UserId.HasValue)
                queryable = queryable.Where(ru => ru.UserId == roleXUserOptions.UserId.Value);

            if (roleXUserOptions.UserIds is not null)
                queryable = queryable.Where(ru => roleXUserOptions.UserIds.Contains(ru.UserId));
        }

        return queryable;
    }

    public async Task<IEnumerable<long>> GetRolesIdAsync(RoleXUserQueryOptions? options = null)
    {
        var set = GetDBSet(options);
        var roleIds = await set
            .Select(x => x.RoleId)
            .ToListAsync();
        return roleIds;
    }

    public async Task<IEnumerable<string>> GetRolesNameAsync(RoleXUserQueryOptions? options = null)
    {
        options = new RoleXUserQueryOptions(options)
        {
            IncludeRole = true
        };

        var set = GetDBSet(options);
        var roleNames = await set
            .Select(x => x.Role!.Name)
            .ToListAsync();
        return roleNames;
    }
}
