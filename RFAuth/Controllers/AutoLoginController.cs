using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/auto-login")]
    public class AutoLoginController(ILoginService loginService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AutoLoginRequest request)
        {
            var response = await loginService.AutoLoginAsync(request);
            response.Attributes = await loginService.DecorateAsync(response, response.Attributes, "LoginAttributes");
            return Ok(response);
        }
    }
}
