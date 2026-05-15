using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFAuthControllers.Exceptions;
using RFAuthIServices.DTO;
using RFAuthIServices.IServices;
using RFPermissionsEntities.Attributes;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("v1/change-password")]
    public class ChangePasswordController(
        ILogger<LoginController> logger,
        IUserPasswordService userPasswordService
    ) : ControllerBase
    {
        [HttpPost]
        [Permission("changePassword")]
        public async Task<IActionResult> PostAsync([FromBody] ChangePasswordRequest data)
        {
            logger.LogInformation("Change password");

            var userId = HttpContext.Items["UserId"] as long?
                ?? throw new NoAuthorizationHeaderException();
            
            if (userId == 0)
                throw new NoAuthorizationHeaderException();

            await userPasswordService.ChangePasswordByUserIdAsync(data.CurrentPassword, data.NewPassword, userId);

            

            return Ok();
        }
    }
}
