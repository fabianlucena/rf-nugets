using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFHttpAction.Entities;
using RFHttpAction.IRepositories;
using RFHttpAction.QueryOptions;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;

namespace RFHttpActionEF.Repositories;

[RegisterService]
public class HttpActionTypeRepository(DbContext context)
    : LocalizableEntityRepository<HttpActionType>(context),
    IHttpActionTypeRepository
{
    public override IQueryable<HttpActionType> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        if (options is HttpActionQueryOptions httpActionOptions)
        {
        }

        return queryable;
    }
}
