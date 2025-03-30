using AutoMapper;
using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFLogger.IServices;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController(
        ILoginService loginService,
        ILoggerService loggerService,
        IMapper mapper
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LoginRequest data)
        {
            var dataCopy = new LoginRequest()
            {
                Username = data.Username,
                Password = "***",
                DeviceToken = data.DeviceToken
            };
            await loggerService.AddInfoGetAsync("Login", new { dataCopy });

            var loginData = await loginService.LoginAsync(data);
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
