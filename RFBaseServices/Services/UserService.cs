using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using RFBaseEntities.Entities;
using RFBaseEntities.Libs;
using RFBaseEntities.QueryOptions;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;
using System.Security.Cryptography;

namespace RFBaseServices.Services
{
    public class UserService(
        IUserRepository userRepository,
        IHttpContextAccessor contextAccessor
    )
        : CommonEntityService<User>(userRepository),
        IUserService
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32;  // 256 bits
        private const int Iterations = 100_000;

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] key = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: KeySize
            );

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        public bool CheckPassword(User user, string password)
        {
            var hash = user.PasswordHash;
            var parts = hash.Split('.', 3);

            int iterations = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] key = Convert.FromBase64String(parts[2]);

            byte[] keyToCheck = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: key.Length
            );

            return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
        }

        public async Task<User> GetSingleByUsernameAsync(string username, UserQueryOptions? options = null)
        {
            return await userRepository.GetSingleByUsernameAsync(username, options);
        }

        public async Task<User?> GetSingleOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null)
        {
            return await userRepository.GetSingleOrDefaultByUsernameAsync(username, options);
        }

        public async Task<User> GetSystemUserAsync()
        {
            return await GetSingleByUsernameAsync("system");
        }

        public async Task<User> GetCurrentOrSystemUserAsync()
        {
            var items = contextAccessor.HttpContext?.Items;
            if (items?.TryGetValue("CurrentUser", out var currentUserData) == true
                && currentUserData is User currentUser
                && currentUser is not null
            )
            {
                return currentUser;
            }

            return await GetSystemUserAsync();
        }

        public async Task<long> GetCurrentOrSystemUserIdAsync()
        {
            var items = contextAccessor.HttpContext?.Items;
            if (items?.TryGetValue("CurrentUserId", out var idCurrentUserData) == true
                && idCurrentUserData is long idCurrentUser
                && idCurrentUser > 0
            )
            {
                return idCurrentUser;
            }
         
            var systemUser = await GetSystemUserAsync();
            return systemUser.Id;
        }
    
        public async Task UpdateLastLoginAtByUserIdAsync(long userId)
        {
            var data = new DataDictionary
            {
                { "LastLoginAt", DateTime.UtcNow },
                { "UpdatedById", userId },
            };

            await UpdateByIdAsync(userId, data);
        }
    }
}
