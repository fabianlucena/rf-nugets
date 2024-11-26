using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/auto-login")]
    public class AutoLoginController(
        ILoginService loginService,
        IMapper mapper
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AutoLoginRequest request)
        {
            var loginData = await loginService.AutoLoginAsync(request);
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
