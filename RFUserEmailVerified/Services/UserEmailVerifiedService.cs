using RFService.IRepo;
using RFService.Repo;
using RFService.Services;
using RFUserEmailVerified.Entities;
using RFUserEmailVerified.IServices;

namespace RFUserEmailVerified.Services
{
    public class UserEmailVerifiedService(IRepo<UserEmailVerified> repo)
        : ServiceTimestampsIdUuid<IRepo<UserEmailVerified>, UserEmailVerified>(repo),
            IUserEmailVerifiedService
    {
        public async Task SetIsVerifiedForIdAsync(bool isVerified, long id)
        {
            await UpdateAsync(new { IsVerified = true }, new GetOptions { Filters = { { "Id", id } } });
        }

        public virtual Task<UserEmailVerified?> GetSingleOrDefaultForUserIdAsync(long userId, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["UserId"] = userId;
            return GetSingleOrDefaultAsync(options);
        }
    }
}
