using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using RFAuth.IServices;
using RFL10n;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/logout")]
    public class LogoutController(
        ISessionService sessionService,
        IRFAuthLoggerService loggerService,
        IL10n l10n
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            await loggerService.AddInfoGetAsync("Logout");

            var sessionId = HttpContext.Items["SessionId"] as Int64? ?? 0;
            if (sessionId == 0)
                throw new NoAuthorizationHeaderException();

            var result = await sessionService.CloseForIdAsync(sessionId);
            if (!result)
                throw new ClosingSessionException();

            return Ok(new { message = await l10n._("Session closed") });
        }
    }
}
