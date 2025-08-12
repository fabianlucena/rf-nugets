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
            if (hash.StartsWith("$plain$"))
                return rawPassword == hash[7..];

            return BCrypt.Net.BCrypt.Verify(rawPassword, hash);
        }

        public bool Verify(string rawPassword, Password password)
            => Verify(rawPassword, password.Hash);

        public async Task<Password> GetSingleForUserIdAsync(Int64 userId)
            => await repo.GetSingleAsync(new QueryOptions { Filters = { { "UserId", userId } } });

        
        public async Task<Password?> GetSingleOrDefaultForUserIdAsync(Int64 userId)
            => await repo.GetSingleOrDefaultAsync(new QueryOptions { Filters = { { "UserId", userId } } });

        
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

        public async Task<int> UpdateForUserIdAsync(IDataDictionary data, Int64 userId, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("UserId", userId);
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
