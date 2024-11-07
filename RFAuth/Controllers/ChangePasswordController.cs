using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using Microsoft.AspNetCore.Http;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/change-password")]
    public class ChangePasswordController(IPasswordService passwordService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ChangePasswordRequest request)
        {
            var userId = HttpContext.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            var password = await passwordService.GetSingleForUserIdAsync(userId.Value);
            var check = passwordService.Verify(request.CurrentPassword, password);
            if (!check)
                throw new BadPasswordException();

            await passwordService.UpdateForIdAsync(
                new { Hash = passwordService.Hash(request.NewPassword) },
                password.Id
            );

            return Ok();
        }
    }
}
