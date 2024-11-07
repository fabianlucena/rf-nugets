using Microsoft.AspNetCore.Mvc;
using RFRegister.DTO;
using RFRegister.IServices;

namespace RFRegister.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class RegisterController(IRegisterService registerService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] RegisterRequest registerData)
        {
            await registerService.RegisterAsync(registerData);
            return Ok();
        }
    }
}
