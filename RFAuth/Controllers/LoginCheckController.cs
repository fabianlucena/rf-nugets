using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RFAuth.Exceptions;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/login-check")]
    public class LoginCheckController() : ControllerBase
    {
        [HttpPost]
        public IActionResult PostAsync()
        {
            var userId = HttpContext.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            return Ok();
        }
    }
}
