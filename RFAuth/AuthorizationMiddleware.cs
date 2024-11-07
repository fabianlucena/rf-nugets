using Microsoft.AspNetCore.Http;
using RFAuth.IServices;

namespace RFAuth
{
    public class AuthorizationMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, ISessionService sessionService)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationList) && authorizationList.Count > 0)
            {
                foreach (var authorization in authorizationList)
                {
                    if (String.IsNullOrEmpty(authorization) || !authorization[..7].Equals("bearer ", StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    var token = authorization[7..].Trim();
                    var session = await sessionService.GetForTokenOrDefaultAsync(token);
                    if (session != null)
                    {
                        context.Items["Session"] = session;
                        context.Items["SessionId"] = session.Id;
                        context.Items["UserId"] = session.UserId;
                    }
                }
            }

            await next(context);
        }
    }
}
