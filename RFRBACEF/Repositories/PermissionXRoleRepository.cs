using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.QueryOptions;
using RFRegisterService.Attributes;

namespace RFRBACEF.Repositories;

[RegisterService]
public class PermissionXRoleRepository(DbContext context)
    : CommonJoinRepository<PermissionXRole>(context),
    IPermissionXRoleRepository
{
    public override IQueryable<PermissionXRole> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        queryable = queryable.OrderBy(ir => ir.PermissionId);

        if (options is PermissionXRoleQueryOptions sessionOptions)
        {
            if (sessionOptions.IncludePermission)
                queryable = queryable.Include(pr => pr.Permission);

            if (sessionOptions.IncludeRole)
                queryable = queryable.Include(pr => pr.Role);
        }

        return queryable;
    }

    public async Task<IEnumerable<long>> GetPermissionsIdByRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null)
    {
        var set = GetDBSet(options);
        var result = await set
            .Where(r => rolesId.Contains(r.RoleId))
            .Select(r => r.PermissionId)
            .ToListAsync();

        return result;
    }
}
