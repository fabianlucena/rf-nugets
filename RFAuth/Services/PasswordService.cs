using RFAuth.Entities;
using RFAuth.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFAuth.Services
{
    public class PasswordService(IRepo<Password> repo)
        : ServiceTimestampsIdUuid<IRepo<Password>, Password>(repo),
            IPasswordService
    {
        public string Hash(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }

        public bool Verify(string rawPassword, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, hash);
        }

        public bool Verify(string rawPassword, Password password)
        {
            return Verify(rawPassword, password.Hash);
        }

        public async Task<Password> GetSingleForUserIdAsync(Int64 userId)
        {
            return await repo.GetSingleAsync(new GetOptions { Filters = { { "UserId", userId } } });
        }

        public async Task<Password?> GetSingleOrDefaultForUserIdAsync(Int64 userId)
        {
            return await repo.GetSingleOrDefaultAsync(new GetOptions { Filters = { { "UserId", userId } } });
        }

        public async Task<Password> GetSingleForUserAsync(User user)
        {
            return await GetSingleForUserIdAsync(user.Id);
        }

        public async Task<Password?> GetSingleOrDefaultForUserAsync(User user)
        {
            return await GetSingleOrDefaultForUserIdAsync(user.Id);
        }

        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("UserId", out object? value))
            {
                options = new GetOptions(options);
                if (value != null
                    && (Int64)value != 0
                )
                {
                    options.Filters = new Dictionary<string, object?> { { "UserId", value } };
                    return options;
                }
                else
                {
                    options.Filters.Remove("UserId");
                }
            }

            return base.SanitizeForAutoGet(options);
        }
    }
}
