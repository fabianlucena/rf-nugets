using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFAuthIServices.DTO;
using RFAuthIServices.IServices;
using RFBaseEntities.Libs;
using RFBaseEntities.ILibs;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController(
        ILoginService loginService,
        ILogger<LoginController> logger,
        IDecoratorsBus decoratorsBus,
        IServiceProvider serviceProvider
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            logger.LogInformation("Login {@Data}", new
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
            var decorated = await decoratorsBus.DecorateAsync("LoginResponse", response, serviceProvider, session);

            return Ok(decorated);
        }
    }
}
