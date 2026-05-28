using Microsoft.EntityFrameworkCore;
using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFAuthIRepositories.Repositories;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;

namespace RFAuthEF.Repositories
{
    public class UserTypeRepository(DbContext context)
        : LocalizableEntityRepository<UserType>(context),
        IUserTypeRepository
    {
        public override IQueryable<UserType> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            return queryable;
        }
    }
}
