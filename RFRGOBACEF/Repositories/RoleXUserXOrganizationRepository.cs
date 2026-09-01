using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;
using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;
using RFRGOBAC.QueryOptions;

namespace RFRGOBACEF.Repositories;

[RegisterService]
public class RoleXUserXOrganizationRepository(DbContext context)
    : CommonJoinRepository<RoleXUserXOrganization>(context),
    IRoleXUserXOrganizationRepository
{
    public override IQueryable<RoleXUserXOrganization> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        queryable = queryable.OrderBy(ruo => ruo.UserId);

        if (options is RoleXUserXOrganizationQueryOptions roleXUserXOrganizationOptions)
        {
            if (roleXUserXOrganizationOptions.IncludeRole)
                queryable = queryable.Include(ruo => ruo.Role);

            if (roleXUserXOrganizationOptions.IncludeUser)
                queryable = queryable.Include(ruo => ruo.User);

            if (roleXUserXOrganizationOptions.IncludeOrganization)
                queryable = queryable.Include(ruo => ruo.Organization);

            if (roleXUserXOrganizationOptions.RoleId.HasValue)
                queryable = queryable.Where(ruo => ruo.RoleId == roleXUserXOrganizationOptions.RoleId.Value);

            if (roleXUserXOrganizationOptions.RolesId != null)
                queryable = queryable.Where(ruo => roleXUserXOrganizationOptions.RolesId.Contains(ruo.RoleId));

            if (roleXUserXOrganizationOptions.UserId.HasValue)
                queryable = queryable.Where(ruo => ruo.UserId == roleXUserXOrganizationOptions.UserId.Value);

            if (roleXUserXOrganizationOptions.UsersId != null)
                queryable = queryable.Where(ruo => roleXUserXOrganizationOptions.UsersId.Contains(ruo.UserId));

            if (roleXUserXOrganizationOptions.OrganizationId.HasValue)
                queryable = queryable.Where(ruo => ruo.OrganizationId == roleXUserXOrganizationOptions.OrganizationId.Value);

            if (roleXUserXOrganizationOptions.OrganizationsId != null)
                queryable = queryable.Where(ruo => roleXUserXOrganizationOptions.OrganizationsId.Contains(ruo.OrganizationId));
        }

        return queryable;
    }

    public async Task<IEnumerable<long>> GetIdsAsync(RoleXUserXOrganizationQueryOptions options)
        => await GetDBSet(options)
            .Select(e => e.RoleId)
            .ToListAsync();

    public async Task<IEnumerable<Organization>> GetOrganizationsAsync(RoleXUserXOrganizationQueryOptions options)
    {
        options = options.Clone();
        options.IncludeOrganization = true;
        options.Distinct = true;
        return await GetDBSet(options)
            .Select(e => e.Organization!)
            .ToListAsync();
    }
}
