using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RFAuth.DTO;
using RFAuth.IServices;
using RFAuthControllers.Exceptions;
using RFPermissions.Attributes;

namespace RFAuthControllers.Controllers;

[ApiController]
[Route("v1/change-password")]
public class ChangePasswordController(
    IRFAuthLoggerService loggerService,
    IUserPasswordService userPasswordService
) : ControllerBase
{
    [HttpPost]
    [Permission("changePassword")]
    public async Task<IActionResult> PostAsync([FromBody] ChangePasswordRequest data)
    {
        await loggerService.AddInfoGetAsync("Change password");

        var userId = HttpContext.Items["UserId"] as long?
            ?? throw new NoAuthorizationHeaderException();
        
        if (userId == 0)
            throw new NoAuthorizationHeaderException();

        await userPasswordService.ChangePasswordByUserIdAsync(data.CurrentPassword, data.NewPassword, userId);

        return NoContent();
    }
}
