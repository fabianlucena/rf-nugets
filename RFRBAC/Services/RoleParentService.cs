using RFOperators;
using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class RoleParentService(
        IRepo<RoleParent> repo
    ) : ServiceTimestamps<IRepo<RoleParent>, RoleParent>(repo),
            IRoleParentService
    {
        public async Task<IEnumerable<Int64>> GetParentsIdForRolesIdAsync(
            IEnumerable<Int64> rolesId,
            GetOptions? options = null
        )
        {
            options ??= new GetOptions();
            options.AddFilter("RoleId", rolesId);
            var rolesParents = await GetListAsync(options);
            return rolesParents.Select(i => i.ParentId);
        }

        public async Task<IEnumerable<Int64>> GetAllRolesIdForRolesIdAsync(
            IEnumerable<Int64> rolesId,
            GetOptions? options = null
        )
        {
            var allRolesId = rolesId.ToList();

            options ??= new GetOptions();
            options.Filters.Add(Op.NE("ParentId", allRolesId));
            var newRolesId = await GetParentsIdForRolesIdAsync(rolesId, options);
            while (newRolesId.Any())
            {
                allRolesId.AddRange(newRolesId);
                options.Filters.Add(Op.NE("ParentId", allRolesId));
                newRolesId = await GetParentsIdForRolesIdAsync(rolesId, options);
            }

            return allRolesId;
        }
    }
}
