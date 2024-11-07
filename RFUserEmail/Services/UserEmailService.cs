using RFService.IRepo;
using RFService.Repo;
using RFService.Services;
using RFUserEmail.Entities;
using RFUserEmail.IServices;

namespace RFUserEmail.Services
{
    public class UserEmailService(IRepo<UserEmail> repo)
        : ServiceTimestampsIdUuid<IRepo<UserEmail>, UserEmail>(repo),
            IUserEmailService
    {
        public override async Task<UserEmail> ValidateForCreationAsync(UserEmail data)
        {
            return await base.ValidateForCreationAsync(data);
        }

        public async Task SetIsVerifiedForIdAsync(bool isVerified, Int64 id)
        {
            await UpdateAsync(new { IsVerified = true }, new GetOptions { Filters = { { "Id", id } } });
        }

        public virtual Task<UserEmail?> GetSingleOrDefaultForUserIdAsync(Int64 userId, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["UserId"] = userId;
            return GetSingleOrDefaultAsync(options);
        }
    }
}
