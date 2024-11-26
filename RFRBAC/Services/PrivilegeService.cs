using Microsoft.AspNetCore.Http;
using RFAuth.Exceptions;
using RFRBAC.IServices;

namespace RFRBAC.Services
{
    public class PrivilegeService(
        IHttpContextAccessor httpContextAccessor,
        IUserRoleService userRoleService,
        IUserPermissionService userPermissionService
    )
        : IPrivilegeService
    {
        private HttpContext? HttpContext => httpContextAccessor?.HttpContext;

        public Task<bool> UserIdHasAnyRoleAsync(Int64 userId, params string[] checkingRoles)
            => userRoleService.UserIdHasAnyRoleAsync(userId, checkingRoles);

        public Task<bool> UserIdHasAnyPermissionAsync(Int64 userId, params string[] checkinPermission)
            => userPermissionService.UserIdHasAnyPermissionAsync(userId, checkinPermission);

        public async Task<bool> HasAnyRoleAsync(params string[] checkingRoles)
        {
            var userId = HttpContext?.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            return await UserIdHasAnyRoleAsync(userId.Value, checkingRoles);
        }

        public async Task<bool> HasAnyPermissionAsync(params string[] checkinPermission)
        {
            var userId = HttpContext?.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            return await UserIdHasAnyPermissionAsync(userId.Value, checkinPermission);
        }
    }
}
