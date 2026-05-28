using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFAuth.IServices;
using RFAuthControllers.Exceptions;

namespace RFAuthControllers.Controllers
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

            var userId = HttpContext.Items["UserId"] as long?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            return Ok();
        }
    }
}
