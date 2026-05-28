using Microsoft.AspNetCore.Mvc;
using RFAuth.IServices;
using RFAuth.DTO;
using RFBase.Libs;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController(
        ILoginService loginService,
        IRFAuthLoggerService loggerService
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            await loggerService.AddInfoGetAsync("Login", () => new
            {
                Username = request.Username,
                Password = "****",
                DeviceToken = request.DeviceToken
            });

            var sessionData = new DataDictionary
            {
                { "ip", Request.Headers["X-Forwarded-For"].FirstOrDefault()
                    ?? HttpContext.Connection.RemoteIpAddress?.ToString() },

                { "userAgent", Request.Headers.UserAgent.ToString() },
            };

            var session = await loginService.LoginAsync(request, sessionData);
            var response = new SessionResponse(session);

            return Ok(response);
        }
    }
}
