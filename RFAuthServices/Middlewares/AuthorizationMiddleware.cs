using Microsoft.AspNetCore.Http;
using RFAuthEntities.QueryOptions;
using RFAuthIServices.IServices;
using RFAuthServices.DTO;

namespace RFAuthServices.Middlewares
{
    public class AuthorizationMiddleware(RequestDelegate next)
    {
        private static readonly Dictionary<string, CachedSession> cache = [];

        public async Task InvokeAsync(
            HttpContext context,
            ISessionService sessionService
        )
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationList)
                && authorizationList.Count == 1)
            {
                var cachedSession = await CheckAuthorizationAsync(
                    authorizationList[0],
                    sessionService
                );
                if (cachedSession is not null)
                {
                    foreach (var item in cachedSession.Items)
                    {
                        context.Items[item.Key] = item.Value;
                    }

                    await sessionService.UpdateLastUsageAsync(cachedSession.SessionId);
                }
            }

            await next(context);
        }

        private static async Task<CachedSession?> CheckAuthorizationAsync(
            string? authorization,
            ISessionService sessionService
        )
        {
            if (String.IsNullOrEmpty(authorization) || !authorization[..7].Equals("bearer ", StringComparison.CurrentCultureIgnoreCase))
                return null;

            var token = authorization[7..].Trim();
            var cachedSession = await GetCachedSessionByTokenAsync(
                token,
                sessionService
            );
            if (cachedSession is null)
                return cachedSession;

            if (cachedSession.ExpireAt < DateTime.UtcNow)
            {
                cache.Remove(token);
                return null;
            }

            await sessionService.UpdateLastUsageAsync(cachedSession.SessionId);

            return cachedSession;
        }

        private static async Task<CachedSession?> GetCachedSessionByTokenAsync(
            string token,
            ISessionService sessionService
        )
        {
            if (cache.TryGetValue(token, out var cachedSession)
                && cachedSession is not null)
            {
                return cachedSession;
            }

            var session = await sessionService.GetFirstOrDefaultByTokenAsync(token, new SessionQueryOptions { IncludeUser = true, IncludeDevice = true });
            if (session is null || session.ExpireAt < DateTime.UtcNow || session.ClosedAt is not null)
                return null;

            session = await sessionService.DecorateAsync(session);

            cachedSession = new CachedSession(session);
            cachedSession.Items["SessionId"] = session.Id;
            cachedSession.Items["UserId"] = session.UserId;
            cachedSession.Items["Session"] = session;
            cachedSession.Items["User"] = session.User;
            cachedSession.Items["Device"] = session.Device;
            if (session.Data is not null)
            {
                cachedSession.Items["RoleIds"] = session.Data["roleIds"];
                cachedSession.Items["RoleNames"] = session.Data["roleNames"];
                cachedSession.Items["PermissionNames"] = session.Data["permissionNames"];
                if (session.Data["currentOrganization"] is not null)
                    cachedSession.Items["CurrentOrganization"] = session.Data["currentOrganization"];

                if (session.Data["currentOrganizationId"] is not null)
                    cachedSession.Items["CurrentOrganizationId"] = session.Data["currentOrganizationId"];
            }

            cache[token] = cachedSession;

            return cachedSession;
        }
    }
}
