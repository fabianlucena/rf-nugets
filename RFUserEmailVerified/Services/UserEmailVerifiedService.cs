using RFService.IRepo;
using RFService.Libs;
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
            await UpdateAsync(new DataDictionary { { "IsVerified", true } }, new GetOptions { Filters = { { "Id", id } } });
        }

        public virtual Task<UserEmailVerified?> GetSingleOrDefaultForUserIdAsync(long userId, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("UserId", userId);
            return GetSingleOrDefaultAsync(options);
        }
    }
}
