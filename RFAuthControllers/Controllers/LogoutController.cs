using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFAuthControllers.Exceptions;
using RFAuthIServices.IServices;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("logout")]
    public class LogoutController(
        ILogger<LoginController> logger,
        ISessionService sessionService
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            logger.LogInformation("Logout");

            var sessionId = HttpContext.Items["SessionId"] as Int64?
                ?? throw new NoAuthorizationHeaderException();

            if (sessionId == 0)
                throw new NoAuthorizationHeaderException();

            await sessionService.CloseByIdAsync(sessionId);

            return Ok();
        }
    }
}
