using Microsoft.AspNetCore.Http;
using RFAuthEntities.QueryOptions;
using RFAuthIServices.IServices;
using RFAuthServices.DTO;
using RFBaseEntities.ILibs;

namespace RFAuthServices.Middlewares
{
    public class AuthorizationMiddleware(RequestDelegate next)
    {
        private static readonly Dictionary<string, CachedSession> cache = [];

        public async Task InvokeAsync(
            HttpContext context,
            ISessionService sessionService,
            IServiceProvider serviceProvider,
            IDecoratorsBus decoratorsBus
        )
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationList)
                && authorizationList.Count == 1)
            {
                var cachedSession = await CheckAuthorizationAsync(
                    authorizationList[0],
                    sessionService,
                    serviceProvider,
                    decoratorsBus
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
            ISessionService sessionService,
            IServiceProvider serviceProvider,
            IDecoratorsBus decoratorsBus
        )
        {
            if (String.IsNullOrEmpty(authorization) || !authorization[..7].Equals("bearer ", StringComparison.CurrentCultureIgnoreCase))
                return null;

            var token = authorization[7..].Trim();
            var cachedSession = await GetCachedSessionByTokenAsync(
                token,
                sessionService,
                serviceProvider,
                decoratorsBus
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
            ISessionService sessionService,
            IServiceProvider serviceProvider,
            IDecoratorsBus decoratorsBus
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

            cachedSession = new CachedSession(session);
            await decoratorsBus.DecorateAsync("CheckAutorization", cachedSession, serviceProvider, session);

            cache[token] = cachedSession;

            return cachedSession;
        }
    }
}
