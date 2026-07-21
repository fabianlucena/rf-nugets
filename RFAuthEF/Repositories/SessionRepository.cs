using Microsoft.EntityFrameworkCore;
using RFAuth.Entities;
using RFAuth.IRepositories;
using RFAuth.QueryOptions;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;

namespace RFAuthEF.Repositories;

[RegisterService]
public class SessionRepository(DbContext context)
    : CreatableEntityRepository<Session>(context),
    ISessionRepository
{
    public override IQueryable<Session> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        if (options is SessionQueryOptions sessionOptions)
        {
            if (sessionOptions.IncludeUser)
                queryable = queryable.Include(u => u.User);

            if (sessionOptions.IncludeDevice)
                queryable = queryable.Include(d => d.Device);

            if (sessionOptions.AuthorizationToken != null)
                queryable = queryable.Where(s => s.AuthorizationToken == sessionOptions.AuthorizationToken);

            if (sessionOptions.AutoLoginToken != null)
                queryable = queryable.Where(s => s.AutoLoginToken == sessionOptions.AutoLoginToken);
        }

        return queryable;
    }
}
