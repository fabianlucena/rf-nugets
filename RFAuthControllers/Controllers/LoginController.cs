using Microsoft.AspNetCore.Mvc;
using RFAuth.IServices;
using RFAuth.DTO;

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

            dynamic sessionData = new SessionData();
            sessionData.ip = Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString();
            sessionData.userAgent = Request.Headers.UserAgent.ToString();
            
            var session = await loginService.LoginAsync(request, sessionData);
            var response = new SessionResponse(session);

            return Ok(response);
        }
    }
}
