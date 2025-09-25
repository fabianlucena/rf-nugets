using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RFAuth.DTO;
using RFAuth.IServices;
using System.Text.Json;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/auto-login")]
    public class AutoLoginController(
        ILoginService loginService,
        IRFAuthLoggerService loggerService,
        IMapper mapper
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AutoLoginRequest data)
        {
            await loggerService.AddInfoGetAsync("Autologin", new { data });

            var sessionData = new
            {
                ip = Request.Headers["X-Forwarded-For"].FirstOrDefault()
                    ?? HttpContext.Connection.RemoteIpAddress?.ToString(),

                userAgent = Request.Headers.UserAgent.ToString(),
            };

            var loginData = await loginService.AutoLoginAsync(
                data,
                JsonSerializer.Serialize(sessionData)
            );
            loginData.Attributes = await loginService.DecorateItemAsync(
                loginData,
                "LoginAttributes",
                loginData.Attributes,
                "Reponse"
            );

            var response = mapper.Map<LoginData, LoginResponse>(loginData);

            return Ok(response);
        }
    }
}
