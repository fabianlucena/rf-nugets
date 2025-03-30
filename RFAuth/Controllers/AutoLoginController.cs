using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using RFLogger.IServices;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/auto-login")]
    public class AutoLoginController(
        ILoginService loginService,
        ILoggerService loggerService,
        IMapper mapper
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AutoLoginRequest data)
        {
            await loggerService.AddInfoGetAsync("Autologin", new { data });

            var loginData = await loginService.AutoLoginAsync(data);
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
