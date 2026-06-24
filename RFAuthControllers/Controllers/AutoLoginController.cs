using Microsoft.AspNetCore.Mvc;
using RFAuth.DTO;
using RFAuth.IServices;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("v1/auto-login")]
    public class AutoLoginController(
        IRFAuthLoggerService loggerService,
        ILoginService loginService
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AutoLoginRequest request)
        {
            await loggerService.AddInfoGetAsync("Autologin", request);

            dynamic sessionData = new SessionData();
            sessionData.ip = Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            sessionData.userAgent = Request.Headers.UserAgent.ToString();

            var session = await loginService.AutoLoginAsync(request, sessionData);
            var response = new SessionResponse(session);

            return Ok(response);
        }
    }
}
