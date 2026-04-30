using Microsoft.EntityFrameworkCore;
using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFAuthIRepositories.Repositories;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;

namespace RFAuthEF.Repositories
{

    public class SessionRepository
        : CreatableEntityRepository<Session>,
        ISessionRepository
    {
        public SessionRepository(DbContext context) : base(context) { }

        public override IQueryable<Session> CreateDBSet(BaseQueryOptions? options)
        {
            var queryable = base.CreateDBSet(options ?? new BaseQueryOptions());

            if (options is SessionQueryOptions sessionOptions)
            {
                if (sessionOptions.IncludeUser)
                {
                    queryable = queryable.Include(u => u.User);
                }

                if (sessionOptions.IncludeDevice)
                {
                    queryable = queryable.Include(d => d.Device);
                }
            }

            return queryable;
        }

        public async Task<Session?> GetFirstOrDefaultByTokenAsync(string token, SessionQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var session = await set
                .Where(s => s.Token == token)
                .FirstOrDefaultAsync();

            return session;
        }

        public async Task<Session?> GetFirstOrDefaultByAutoLoginTokenAsync(string autoLoginToken, SessionQueryOptions? options = null)
        {
            var set = CreateDBSet(options);
            var session = await set
                .Where(s => s.AutoLoginToken == autoLoginToken)
                .FirstOrDefaultAsync();

            return session;
        }
    }
}
