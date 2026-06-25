using Microsoft.AspNetCore.Mvc;
using RFAuth.DTO;
using RFAuth.IServices;
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
                request.Username,
                Password = "****",
                request.DeviceToken
            });

            var clientData = new DataDictionary {
                { "ip", Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString() },

                { "userAgent", Request.Headers.UserAgent.ToString() },
            };

            var session = await loginService.LoginAsync(request, clientData);
            var response = new SessionResponse(session);

            return Ok(response);
        }
    }
}
