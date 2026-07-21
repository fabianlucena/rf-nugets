using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFHttpAction.Entities;
using RFHttpAction.IRepositories;
using RFHttpAction.QueryOptions;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;

namespace RFHttpActionEF.Repositories;

[RegisterService]
public class HttpActionRepository(DbContext context)
    : AuditableEntityRepository<HttpAction>(context),
    IHttpActionRepository
{
    public override IQueryable<HttpAction> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        if (options is HttpActionQueryOptions httpActionOptions)
        {
            if (httpActionOptions.Token is not null)
                queryable = queryable.Where(x => x.Token == httpActionOptions.Token);

            if (httpActionOptions.DataContains is not null)
                queryable = queryable.Where(x => x.Data != null && x.Data.Contains(httpActionOptions.DataContains));

            if (httpActionOptions.IsNotClosed)
                queryable = queryable.Where(x => x.ClosedAt == null);
        }

        return queryable;
    }
}
