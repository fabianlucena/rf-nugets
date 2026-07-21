using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;

namespace RFRGOBACEF.Repositories;

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
