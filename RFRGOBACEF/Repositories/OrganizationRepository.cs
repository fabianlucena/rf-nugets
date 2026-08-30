using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;
using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;

namespace RFRGOBACEF.Repositories;

[RegisterService]
public class OrganizationRepository(DbContext context)
    : ALocalizableEntityRepository<Organization>(context),
    IOrganizationRepository
{
    public override IQueryable<Organization> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        return queryable;
    }
}
