using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using Microsoft.AspNetCore.Http;
using RFLogger.IServices;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/logout")]
    public class LogoutController(
        ISessionService sessionService,
        ILoggerService loggerService
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            await loggerService.AddInfoGetAsync("Logout");

            var sessionIdText = HttpContext.Session.GetString("sessionIdText");
            if (string.IsNullOrEmpty(sessionIdText))
                throw new NoAuthorizationHeaderException();

            return Ok(await sessionService.CloseForIdAsync(Convert.ToInt64(sessionIdText)));
        }
    }
}
