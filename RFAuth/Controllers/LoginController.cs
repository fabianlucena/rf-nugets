using AutoMapper;
using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class LoginController(ILoginService loginService, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LoginRequest request)
        {
            var loginData = await loginService.LoginAsync(request);
            loginData.Attributes = await loginService.DecorateAsync(loginData, loginData.Attributes, "LoginAttributes");

            var response = mapper.Map<LoginData, LoginResponse>(loginData);

            return Ok(response);
        }
    }
}
