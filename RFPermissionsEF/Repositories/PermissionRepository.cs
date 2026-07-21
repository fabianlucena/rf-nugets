using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFPermissions.Entities;
using RFPermissions.IRepositories;
using RFPermissions.QueryOptions;
using RFRegisterService.Attributes;

namespace RFPermissionsEF.Repositories;

[RegisterService]
public class PermissionRepository(DbContext context)
    : CreatableEntityRepository<Permission>(context),
    IPermissionRepository
{
    public override IQueryable<Permission> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        if (options is PermissionQueryOptions permissionOptions)
        {
            if (permissionOptions.Names is not null)
                queryable = queryable.Where(p => permissionOptions.Names.Contains(p.Name));
        }

        return queryable;
    }

    public async Task<IEnumerable<string>> GetNamesAsync(PermissionQueryOptions options)
    {
        return await GetDBSet(options)
            .Select(r => r.Name)
            .ToListAsync();
    }
}
