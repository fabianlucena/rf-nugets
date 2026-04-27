using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFPermissionsEntities.Entities;
using RFPermissionsEntities.QueryOptions;
using RFPermissionsIRepositories.Repositories;

namespace RFPermissionsEF.Repositories
{
    public class PermissionRepository
        : CreatableEntityRepository<Permission>,
        IPermissionRepository
    {
        public PermissionRepository(DbContext context) : base(context) { }

        public async Task<IEnumerable<string>> GetListNameByIdAsync(IEnumerable<long> permissionsId, PermissionQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var result = await set
                .Where(r => permissionsId.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            return result;
        }
    }
}
