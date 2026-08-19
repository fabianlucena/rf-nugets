using Microsoft.Extensions.Logging;
using RFAuth.IServices;
using RFAuth.QueryOptions;
using RFEventBus;
using RFOauth2Client.IServices;

namespace RFOauth2Client.Service;

public class LogoutService(
    ILogger<LogoutService> logger,
    IProviderService providerService,
    ISessionService sessionService
)
{
    [EventHandler]
    public async Task LogoutEvent(Event evt)
    {
        logger.LogInformation("OAuth2 logout event received");

        var data = evt.Data;
        if (data == null)
        {
            logger.LogWarning("Logout event received with no data");
            return;
        }

        var type = data.GetType();
        if (type == null)
        {
            logger.LogWarning("Logout event received with no data");
            return;
        }

        var sessionId = type.GetProperty("SessionId")?.GetValue(data) as long? ?? 0;
        if (sessionId == 0)
        {
            logger.LogWarning("Logout event received with null session ID");
            return;
        }

        var session = await sessionService.GetSingleByIdAsync(sessionId, new SessionQueryOptions { IncludeUser = true });
        if (session == null)
        {
            logger.LogWarning("Logout event received with invalid session");
            return;
        }

        var provider = await providerService.GetSingleOrDefaultByNameAsync(session.IdentityProvider);
        if (provider == null)
        {
            logger.LogWarning("Logout event received with no valid provider");
            return;
        }

        await providerService.Logout(provider, session.User!.Username);
    }
}
