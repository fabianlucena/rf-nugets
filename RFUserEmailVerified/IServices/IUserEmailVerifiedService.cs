using RFIServices.IServices;
using RFUserEmailVerified.Entities;
using RFUserEmailVerified.QueryOptions;

namespace RFUserEmailVerified.IServices
{
    public interface IUserEmailVerifiedService : ICommonEntityService<UserEmailVerified>
    {
        Task<UserEmailVerified?> GetSingleOrDefaultByUserIdAsync(long userId, UserEmailVerifiedQueryOptions? options = null);
        Task<UserEmailVerified?> GetSingleOrDefaultByEmailAsync(string email, UserEmailVerifiedQueryOptions? options = null);

        Task SetIsVerifiedByIdAsync(bool isVerified, long id);
    }
}
