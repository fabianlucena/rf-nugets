using RFService.IServices;
using RFService.Repo;
using RFUserEmail.Entities;

namespace RFUserEmail.IServices
{
    public interface IUserEmailService : IServiceId<UserEmail>
    {
        Task<UserEmail?> GetSingleOrDefaultForUserIdAsync(Int64 userId, QueryOptions? options = null);

        Task SetIsVerifiedForIdAsync(bool isVerified, Int64 id);
    }
}
