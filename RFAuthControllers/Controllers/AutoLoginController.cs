using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFAuthIServices.DTO;
using RFAuthIServices.IServices;
using RFBaseEntities.ILibs;
using RFBaseEntities.Libs;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("auto-login")]
    public class AutoLoginController(
        ILogger<LoginController> logger,
        ILoginService loginService,
        IDecoratorsBus decoratorsBus,
        IServiceProvider serviceProvider
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
            var decorated = await decoratorsBus.DecorateAsync("LoginResponse", response, serviceProvider, session);

            return Ok(decorated);
        }
    }
}
