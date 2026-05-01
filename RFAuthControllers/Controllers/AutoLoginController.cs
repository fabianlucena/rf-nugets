using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFAuthIServices.DTO;
using RFAuthIServices.IServices;
using RFBaseEntities.Libs;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("v1/auto-login")]
    public class AutoLoginController(
        ILogger<LoginController> logger,
        ILoginService loginService
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AutoLoginRequest request)
        {
            logger.LogInformation("Login {@Data}", request);

            var sessionData = new DataDictionary
            {
                { "ip", Request.Headers["X-Forwarded-For"].FirstOrDefault()
                    ?? HttpContext.Connection.RemoteIpAddress?.ToString() },

                { "userAgent", Request.Headers.UserAgent.ToString() },
            };

            var session = await loginService.AutoLoginAsync(request, sessionData);
            var response = new SessionResponse(session);

            return Ok(response);
        }
    }
}
