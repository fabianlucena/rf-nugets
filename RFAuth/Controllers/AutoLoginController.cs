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
            response.Attributes = await loginService.DecorateItemAsync(
                response,
                "LoginAttributes",
                response.Attributes,
                "Reponse"
            );
            return Ok(response);
        }
    }
}
