using AutoMapper;
using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Services;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController(ILoginService loginService, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LoginRequest request)
        {
            var loginData = await loginService.LoginAsync(request);
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
