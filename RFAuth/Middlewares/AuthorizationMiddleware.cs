using Microsoft.AspNetCore.Http;
using RFAuth.DTO;
using RFAuth.IServices;
using RFAuth.QueryOptions;
using RFBase.ILibs;
using RFEventBus;

namespace RFAuth.Middlewares;

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

        var session = await sessionService.GetSingleOrDefaultByAuthorizationTokenAsync(token, new SessionQueryOptions { IncludeUser = true, IncludeDevice = true });
        if (session is null || session.ExpireAt < DateTime.UtcNow || session.ClosedAt is not null)
            return null;

        session = await sessionService.DecorateAsync(session);

        cachedSession = new CachedSession(session);
        cachedSession.Items["SessionId"] = session.Id;
        cachedSession.Items["UserId"] = session.UserId;
        cachedSession.Items["Session"] = session;
        cachedSession.Items["User"] = session.User;
        cachedSession.Items["Device"] = session.Device;
        
        foreach (var kv in session.ResponseData)
            cachedSession.Items[kv.Key] = kv.Value;

        foreach (var kv in session.InternalData)
            cachedSession.Items[kv.Key] = kv.Value;

        cache[token] = cachedSession;

        return cachedSession;
    }

    [EventHandler]
    public static void SessionUpdated(Event evt)
    {
        if (evt.Data is not IDataDictionary data)
            return;

        if (!data.TryGetValue("SessionId", out var sessionIdObj))
            return;

        if (sessionIdObj is not long sessionId)
            return;

        var token = cache.FirstOrDefault(x => x.Value?.SessionId == sessionId)
            .Key;

        if (token == null)
            return;

        cache.Remove(token);
    }
}