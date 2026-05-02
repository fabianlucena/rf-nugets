using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseEF.Repositories
{
    public class NominableEntityRepository<T>(DbContext context)
        : CommonEntityRepository<T>(context)
        where T : NominableEntity, new()
    {
        public override IQueryable<T> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            return queryable;
        }

        public async Task<T?> GetSingleOrDefaultByNameAsync(string name, BaseQueryOptions? options = null)
        {
            var queryable = GetDBSet(options);
            var result = await queryable
                .Where(e => e.Name == name)
                .SingleOrDefaultAsync();

            return result;
        }

        public async Task<long?> GetSingleOrDefaultIdByNameAsync(string name, BaseQueryOptions? options = null)
        {
            var queryable = GetDBSet(options);
            var result = await queryable
                .Where(e => e.Name == name)
                .Select(e => e.Id)
                .SingleOrDefaultAsync();

            return result;
        }
    }
}
