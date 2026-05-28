using Microsoft.Extensions.DependencyInjection;
using RFAuth.Entities;
using RFAuth.QueryOptions;
using RFAuth.IServices;
using System.Security.Cryptography;
using RFAuth.IRepositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using RFEntities.Entities;
using RFBase.ILibs;
using RFBase.Libs;
using RFIServices.IServices;
using RFAuth.Exceptions;
using RFServices.Services;
using RFServices.Exceptions;

namespace RFAuth.Services
{
    public class UserPasswordService(
        IUserPasswordRepository userPasswordRepository,
        IServiceProvider serviceProvider
    )
        : NoIdEntityService<UserPassword>(userPasswordRepository),
        IUserPasswordService
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32;  // 256 bits
        private const int Iterations = 100_000;

        public static string HashPassword(string password)
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

        public static bool CheckPassword(string hash, string password)
        {
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

        public async Task<UserPassword?> GetSingleOrDefaultByUserIdAsync(long userId)
        {
            var users = await GetListAsync(
                new UserPasswordQueryOptions
                {
                    UserId = userId,
                    Take = 2,
                }
            );

            if (!users.Any())
                return null;

            if (users.Count() > 1)
                throw new MultiplePasswordEntriesFoundForUserIdException(userId);

            return users.First();
        }

        public async Task<UserPassword> GetSingleByUserIdAsync(long userId)
            =>  await GetSingleOrDefaultByUserIdAsync(userId)
                ?? throw new PasswordForUserIdNotFoundException(userId);

        public async Task<UserPassword> GetSingleByUserAsync(User user)
            => await GetSingleByUserIdAsync(user.Id);
        
        public async Task<UserPassword?> GetSingleOrDefaultByUserAsync(User user)
            => await GetSingleOrDefaultByUserIdAsync(user.Id);

        public async Task<int> UpdateByUserIdAsync(IDataDictionary data, long userId)
        {
            return await UpdateByUserIdAsync(data, userId);
        }

        public async Task<bool> CreateOrUpdateByUserIdAsync(string password, long userId)
        {
            var pasaswordObj = await GetSingleOrDefaultByUserIdAsync(userId);
            if (pasaswordObj == null)
            {
                pasaswordObj = await CreateAsync(new UserPassword
                {
                    UserId = userId,
                    PasswordHash = HashPassword(password)
                });

                return pasaswordObj != null;
            }

            var result = await UpdateByUserIdAsync(
                new DataDictionary { { "PasswordHash", HashPassword(password) } },
                userId
            );

            return result > 0;
        }

        public async Task<bool> CreateOrUpdateByUsernameAsync(string password, string username)
        {
            var userService = serviceProvider.GetRequiredService<IUserService>();
            var userId = await userService.GetSingleIdByUsernameAsync(username);
            return await CreateOrUpdateByUserIdAsync(password, userId);
        }

        public async Task<bool> CheckPasswordByUserIdAsync(string password, long userId)
        {
            var userPassword = await GetSingleByUserIdAsync(userId);
            var check = CheckPassword(userPassword.PasswordHash, password);
            if (!check)
                throw new BadCurrentPasswordException();

            return true;
        }


        public async Task<bool> ChangePasswordByUserIdAsync(string currentPassword, string newPassword, long userId)
        {
            var userPassword = await GetSingleByUserIdAsync(userId);
            var check = CheckPassword(userPassword.PasswordHash, currentPassword);
            if (!check)
                throw new BadCurrentPasswordException();

            await UpdateByUserIdAsync(
                new DataDictionary { { "PasswordHash", HashPassword(newPassword) } },
                userId
            );

            return true;
        }

        public async Task<bool> CreateIfNotExistsByUsernameAsync(string password, string username)
        {
            var userService = serviceProvider.GetRequiredService<IUserService>();
            var userId = await userService.GetSingleIdByUsernameAsync(username);

            var userPassword = await GetSingleOrDefaultByUserIdAsync(userId);
            if (userPassword != null)
                return false;

            await CreateAsync(new UserPassword
            {
                UserId = userId,
                PasswordHash = HashPassword(password)
            });

            return true;
        }
    }
}
