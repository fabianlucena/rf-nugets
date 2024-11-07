using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using Microsoft.AspNetCore.Http;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class LogoutController(ISessionService sessionService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            var sessionIdText = HttpContext.Session.GetString("sessionIdText");
            if (string.IsNullOrEmpty(sessionIdText))
                throw new NoAuthorizationHeaderException();

            return Ok(await sessionService.CloseForIdAsync(Convert.ToInt64(sessionIdText)));
        }
    }
}
