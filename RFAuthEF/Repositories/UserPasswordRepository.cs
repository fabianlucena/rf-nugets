using Microsoft.EntityFrameworkCore;
using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFAuthIRepositories.Repositories;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;

namespace RFAuthEF.Repositories
{
    public class UserPasswordRepository(DbContext context)
        : NoIdEntityRepository<UserPassword>(context),
        IUserPasswordRepository
    {
        public override IQueryable<UserPassword> CreateDBSet(BaseQueryOptions? options)
        {
            var queryable = base.CreateDBSet(options);

            queryable = queryable.OrderBy(up => up.UserId);

            if (options is UserPasswordQueryOptions userPasswordOptions)
            {
                if (userPasswordOptions.IncludeUser)
                    queryable = queryable.Include(up => up.User);

                if (userPasswordOptions.UserId is not null)
                    queryable = queryable.Where(up => up.UserId == userPasswordOptions.UserId);
            }

            return queryable;
        }
    }
}
