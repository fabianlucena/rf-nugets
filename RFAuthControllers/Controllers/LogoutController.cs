using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RFAuth.IServices;
using RFAuthControllers.Exceptions;
using RFL10n;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("v1/logout")]
    public class LogoutController(
        IRFAuthLoggerService loggerService,
        ISessionService sessionService,
        IL10n l10n
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

            return Ok(new { message = await l10n._("Session closed") });
        }
    }
}
