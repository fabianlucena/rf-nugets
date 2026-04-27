using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;

namespace RFRBACEF.Repositories
{
    public class RoleRepository
        : CreatableEntityRepository<Role>,
        IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context) { }

        public async Task<IEnumerable<string>> GetListNamesByIdAsync(IEnumerable<long> ids, RoleQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var list = await set
                .Where(r => ids.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            return list;
        }
    }
}
