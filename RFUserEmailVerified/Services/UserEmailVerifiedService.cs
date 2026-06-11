using RFBase.Libs;
using RFServices.Services;
using RFUserEmailVerified.Entities;
using RFUserEmailVerified.IRepositories;
using RFUserEmailVerified.IServices;
using RFUserEmailVerified.QueryOptions;

namespace RFUserEmailVerified.Services;

public class UserEmailVerifiedService(
    IUserEmailVerifiedRepository UserEmailVerifiedRepository,
    IServiceProvider serviceProvider
)
    : CommonEntityService<UserEmailVerified>(UserEmailVerifiedRepository, serviceProvider),
    IUserEmailVerifiedService
{
    public Task<UserEmailVerified?> GetSingleOrDefaultByUserIdAsync(long userId, UserEmailVerifiedQueryOptions? options = null)
    {
        options = new UserEmailVerifiedQueryOptions(options)
        {
            UserId = userId
        };

        return GetSingleOrDefaultAsync(options);
    }

    public Task<UserEmailVerified?> GetSingleOrDefaultByEmailAsync(string email, UserEmailVerifiedQueryOptions? options = null)
    {
        options = new UserEmailVerifiedQueryOptions(options)
        {
            Email = email
        };

        return GetSingleOrDefaultAsync(options);
    }

    public async Task SetIsVerifiedByIdAsync(bool isVerified, long id)
    {
        var options = new UserEmailVerifiedQueryOptions
        {
            Id = id
        };

        await UpdateAsync(new DataDictionary { { "IsVerified", isVerified } }, options);
    }
}
