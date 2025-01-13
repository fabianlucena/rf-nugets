using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using Microsoft.AspNetCore.Http;
using RFService.Authorization;
using RFService.Libs;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/change-password")]
    public class ChangePasswordController(
        IPasswordService passwordService
    ) : ControllerBase
    {
        [HttpPost]
        [Permission("changePassword")]
        public async Task<IActionResult> PostAsync([FromBody] ChangePasswordRequest request)
        {
            var userId = HttpContext.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            var password = await passwordService.GetSingleForUserIdAsync(userId.Value);
            var check = passwordService.Verify(request.CurrentPassword, password);
            if (!check)
                throw new BadCurrentPasswordException();

            await passwordService.UpdateForIdAsync(
                password.Id,
                new DataDictionary { { "Hash", passwordService.Hash(request.NewPassword) } }
            );

            return Ok();
        }
    }
}
