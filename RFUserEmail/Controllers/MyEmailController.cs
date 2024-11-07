using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFUserEmail.DTO;
using RFUserEmail.IServices;
using RFUserEmail.Entities;
using RFAuth.Exceptions;
using RFService.Authorization;
using RFService.Repo;

namespace RFUserEmail.Controllers
{
    [ApiController]
    [Route("v1/my-email")]
    public class MyEmailController(
        IUserEmailService userEmailService
    ) : ControllerBase
    {
        [HttpPost]
        [Permission("myEmail.create")]
        public async Task<IActionResult> MyEmailPostAsync([FromBody] AddEmailRequest request)
        {
            var userId = HttpContext.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            var userEmail = new UserEmail {
                UserId = userId.Value,
                Email = request.Email,
            };
            var response = await userEmailService.CreateAsync(userEmail);
            return Ok(response);
        }
    }
}
