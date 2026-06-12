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

            if (roleXUserOptions.UserId.HasValue)
                queryable = queryable.Where(ru => ru.UserId == roleXUserOptions.UserId.Value);
        }

        return queryable;
    }

    public async Task<IEnumerable<long>> GetListRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
    {
        var set = GetDBSet(options);
        var roleIds = await set
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId)
            .ToListAsync();
        return roleIds;
    }

    public async Task<IEnumerable<string>> GetListRoleNamesByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
    {
        options = new RoleXUserQueryOptions(options)
        {
            IncludeRole = true
        };

        var set = GetDBSet(options);
        var roleNames = await set
            .Where(x => x.UserId == userId)
            .Select(x => x.Role!.Name)
            .ToListAsync();
        return roleNames;
    }
}
