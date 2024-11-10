using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Operator;
using RFService.Repo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class RoleParentService(
        IRepo<RoleParent> repo
    ) : Service<IRepo<RoleParent>, RoleParent>(repo),
            IRoleParentService
    {
        public async Task<IEnumerable<Int64>> GetParentsIdForRolesIdAsync(
            IEnumerable<Int64> rolesId,
            GetOptions? options = null
        )
        {
            options ??= new GetOptions();
            options.Filters["RoleId"] = rolesId;
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
            options.Filters["RoleId"] = rolesId;
            options.Filters["ParentId"] = Op.DistinctTo(allRolesId);
            var newRolesId = await GetParentsIdForRolesIdAsync(rolesId);
            while (newRolesId.Any())
            {
                allRolesId.AddRange(newRolesId);
                options.Filters["RoleId"] = newRolesId;
                options.Filters["ParentId"] = Op.DistinctTo(allRolesId);
                newRolesId = await GetParentsIdForRolesIdAsync(rolesId);
            }

            return allRolesId;
        }
    }
}
