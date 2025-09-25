using AutoMapper;
using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController(
        ILoginService loginService,
        IRFAuthLoggerService loggerService,
        IMapper mapper
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LoginRequest request)
        {
            var data = new LoginRequest()
            {
                Username = request.Username,
                Password = "****",
                DeviceToken = request.DeviceToken
            };
            await loggerService.AddInfoGetAsync("Login", new { data });

            var sessionData = new
            {
                ip = Request.Headers["X-Forwarded-For"].FirstOrDefault()
                    ?? HttpContext.Connection.RemoteIpAddress?.ToString(),

                userAgent = Request.Headers.UserAgent.ToString(),
            };
            var loginData = await loginService.LoginAsync(
                request,
                JsonSerializer.Serialize(sessionData)
            );
            loginData.Attributes = await loginService.DecorateItemAsync(
                loginData,
                "LoginAttributes",
                loginData.Attributes,
                "Result"
            );

            var response = mapper.Map<LoginData, LoginResponse>(loginData);

            return Ok(response);
        }
    }
}
