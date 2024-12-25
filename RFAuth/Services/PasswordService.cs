using Microsoft.Extensions.Options;
using RFAuth.Entities;
using RFAuth.IServices;
using RFService.ILibs;
using RFService.IRepo;
using RFService.Libs;
using RFService.Repo;
using RFService.Services;

namespace RFAuth.Services
{
    public class PasswordService(
        IRepo<Password> repo,
        IUserService userService
    )
        : ServiceTimestampsIdUuid<IRepo<Password>, Password>(repo),
            IPasswordService
    {
        public string Hash(string rawPassword)
            => BCrypt.Net.BCrypt.HashPassword(rawPassword);

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
                    options.Filters = new DataDictionary { { "UserId", value } };
                    return options;
                }
                else
                {
                    options.Filters.Remove("UserId");
                }
            }

            return base.SanitizeForAutoGet(options);
        }

        public async Task<int> UpdateForUserIdAsync(IDataDictionary data, Int64 userId, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["UserId"] = userId;
            return await UpdateAsync(data, options);
        }

        public async Task<bool> CreateOrUpdateForUserIdAsync(string password, Int64 userId)
        {
            var pasaswordObj = await GetSingleOrDefaultForUserIdAsync(userId);
            if (pasaswordObj == null)
            {
                pasaswordObj = await CreateAsync(new Password
                {
                    UserId = userId,
                    Hash = Hash(password)
                });

                return pasaswordObj != null;
            }

            var result = await UpdateForUserIdAsync(
                new DataDictionary { { "Hash", Hash(password) } },
                userId
            );

            return result > 0;
        }

        public async Task<bool> CreateOrUpdateForUsernameAsync(string password, string username)
        {
            var userId = await userService.GetSingleIdForUsernameAsync(username);
            return await CreateOrUpdateForUserIdAsync(password, userId);
        }
    }
}
