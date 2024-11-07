using RFService.IService;
using RFService.Repo;
using RFUserEmail.Entities;

namespace RFUserEmail.IServices
{
    public interface IUserEmailService : IServiceId<UserEmail>
    {
        Task<UserEmail?> GetSingleOrDefaultForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task SetIsVerifiedForIdAsync(bool isVerified, Int64 id);
    }
}
