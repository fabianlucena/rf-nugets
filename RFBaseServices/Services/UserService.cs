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

        public async Task<long> GetSingleIdByUsernameAsync(string username, UserQueryOptions? options = null)
        {
            return await userRepository.GetSingleIdByUsernameAsync(username, options);
        }
    }
}
