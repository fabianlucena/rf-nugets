using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RFAuth.IServices;
using RFAuthControllers.Exceptions;
using RFEventBus;
using RFL10n;

namespace RFAuthControllers.Controllers;

[ApiController]
[Route("v1/logout")]
public class LogoutController(
    IRFAuthLoggerService loggerService,
    ISessionService sessionService,
    IL10n l10n,
    IEventBus eventBus
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        await loggerService.AddInfoGetAsync("Logout");

        var sessionId = HttpContext.Items["SessionId"] as long?
            ?? throw new NoAuthorizationHeaderException();

        if (sessionId == 0)
            throw new NoAuthorizationHeaderException();

        await sessionService.CloseByIdAsync(sessionId);

        var evt = new Event("Logout", new { SessionId = sessionId });
        _ = eventBus.Publish(evt);

        return Ok(new { message = await l10n._("Session closed") });
    }
}
