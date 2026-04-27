using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFAuthControllers.Exceptions;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("login-check")]
    public class LoginCheckController(
        ILogger<LoginController> logger
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            logger.LogInformation("Check login");

            var userId = HttpContext.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            return Ok();
        }
    }
}
