using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using Microsoft.AspNetCore.Http;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/logout")]
    public class LogoutController(
        ISessionService sessionService,
        IRFAuthLoggerService loggerService
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            await loggerService.AddInfoGetAsync("Logout");

            var sessionId = HttpContext.Items["SessionId"] as Int64? ?? 0;
            if (sessionId == 0)
                throw new NoAuthorizationHeaderException();

            return Ok(await sessionService.CloseForIdAsync(sessionId));
        }
    }
}
