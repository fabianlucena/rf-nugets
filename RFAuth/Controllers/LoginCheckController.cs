using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RFAuth.Exceptions;
using RFAuth.IServices;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/login-check")]
    public class LoginCheckController(
        IRFAuthLoggerService loggerService
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            await loggerService.AddInfoGetAsync("Check login");

            var userId = HttpContext.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            return Ok();
        }
    }
}
