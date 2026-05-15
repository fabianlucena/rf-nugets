using Microsoft.EntityFrameworkCore;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;
using RFRGOBACEntities.Entities;
using RFRGOBACIRepositories.IRepositories;

namespace RFRGOBACEF.Repositories
{
    public class OrganizationRepository(DbContext context)
        : LocalizableEntityRepository<Organization>(context),
        IOrganizationRepository
    {
        public override IQueryable<Organization> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            return queryable;
        }
    }
}
