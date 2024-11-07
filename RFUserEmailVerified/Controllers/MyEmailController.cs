using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFUserEmail.DTO;
using RFUserEmail.IServices;
using RFUserEmail.Entities;
using RFAuth.Exceptions;
using RFService.Authorization;
using RFService.Repo;
using RFHttpAction.IServices;
using RFHttpAction.Entities;

namespace RFUserEmail.Controllers
{
    [ApiController]
    [Route("v1/my-email")]
    public class MyEmailController(
        IUserEmailService userEmailService,
        IHttpActionTypeService httpActionTypeService,
        IHttpActionService httpActionService
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

        [Route("verify")]
        [HttpPost]
        [Permission("myEmail.verify")]
        public async Task<IActionResult> VerifyEmailPostAsync()
        {
            var userId = HttpContext.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            var userEmail = await userEmailService.GetSingleOrDefaultAsync(new GetOptions { Filters = { { "UserId", userId } } })
                ?? throw new UserDoesNotHaveEmailException();

            if (userEmail.IsVerified)
                throw new UserEmailIsAlreadyVerifiedException();

            var action = await httpActionService.CreateAsync(
                new HttpAction
                {
                    TypeId = await httpActionTypeService.GetIdForNameAsync(
                        "userEmail.verify",
                        creator: name => new HttpActionType {
                            Name = name,
                            Title = "UserEmail Verify",
                        }
                    ),
                    Data = userEmail.Id.ToString(),
                }
            );

            return Ok(new
            {
                url = httpActionService.GetUrl(action)
            });
        }
    }
}
