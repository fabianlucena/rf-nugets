using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.QueryOptions;

namespace RFEntitiesEF.Repositories
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
