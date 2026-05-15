using Microsoft.EntityFrameworkCore;
using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFAuthIRepositories.Repositories;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;

namespace RFAuthEF.Repositories
{

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

                if (sessionOptions.Token != null)
                    queryable = queryable.Where(s => s.Token == sessionOptions.Token);

                if (sessionOptions.AutoLoginToken != null)
                    queryable = queryable.Where(s => s.AutoLoginToken == sessionOptions.AutoLoginToken);
            }

            return queryable;
        }
    }
}
