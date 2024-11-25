using Microsoft.AspNetCore.Http;
using RFAuth.Exceptions;
using RFRBAC.IServices;

namespace RFRBAC.Services
{
    public class PrivilegeService(
        IHttpContextAccessor httpContextAccessor,
        IUserRoleService userRoleService
    )
        : IPrivilegeService
    {
        private HttpContext? HttpContext => httpContextAccessor?.HttpContext;

        public async Task<bool> UserIdHasRoleAsync(Int64 userId, params string[] checkingRoles)
        {
            return await userRoleService.UserIdHasAnyRoleAsync(userId, checkingRoles);
        }

        public async Task<bool> HasRoleAsync(params string[] checkingRoles)
        {
            var userId = HttpContext?.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            return await UserIdHasRoleAsync(userId.Value, checkingRoles);
        }
    }
}
