using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;
using RFUserEmailVerified.Entities;
using RFUserEmailVerified.IRepositories;
using RFUserEmailVerified.QueryOptions;

namespace RFUserEmailVerifiedEF.Repositories;

[RegisterService]
public class UserEmailVerifiedRepository(DbContext context)
    : CommonEntityRepository<UserEmailVerified>(context),
    IUserEmailVerifiedRepository
{
    public override IQueryable<UserEmailVerified> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        if (options is UserEmailVerifiedQueryOptions userEmailVerifiedQueryOptions)
        {
            if (userEmailVerifiedQueryOptions.UserId is not null)
                queryable = queryable.Where(uev => uev.UserId == userEmailVerifiedQueryOptions.UserId);

            if (userEmailVerifiedQueryOptions.Email is not null)
                queryable = queryable.Where(uev => uev.Email == userEmailVerifiedQueryOptions.Email);
        }

        return queryable;
    }
}
