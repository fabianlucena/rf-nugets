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
            => await repo.GetSingleAsync(new GetOptions { Filters = { { "UserId", userId } } });

        public async Task<Password?> GetSingleOrDefaultForUserIdAsync(Int64 userId)
            => await repo.GetSingleOrDefaultAsync(new GetOptions { Filters = { { "UserId", userId } } });

        public async Task<Password> GetSingleForUserAsync(User user)
            => await GetSingleForUserIdAsync(user.Id);

        public async Task<Password?> GetSingleOrDefaultForUserAsync(User user)
            => await GetSingleOrDefaultForUserIdAsync(user.Id);

        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
        {
            if (data.TryGetValue("UserId", out object? value))
            {
                if (value != null
                    && (Int64)value != 0
                )
                {
                    return new DataDictionary { { "UserId", value } };
                }
                else
                {
                    data = new DataDictionary(data);
                    data.Remove("UserId");
                }
            }

            return base.SanitizeDataForAutoGet(data);
        }


        public async Task<int> UpdateForUserIdAsync(Int64 userId, IDataDictionary data, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("UserId", userId);
            return await UpdateAsync(data, options);
        }

        public async Task<bool> CreateOrUpdateForUserIdAsync(Int64 userId, string password)
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
                userId,
                new DataDictionary { { "Hash", Hash(password) } }
            );

            return result > 0;
        }

        public async Task<bool> CreateOrUpdateForUsernameAsync(string username, string password)
        {
            var userId = await userService.GetSingleIdForUsernameAsync(username);
            return await CreateOrUpdateForUserIdAsync(userId, password);
        }
    }
}
