using Microsoft.AspNetCore.Mvc;
using RFUserEmail.DTO;
using RFAuth.Exceptions;
using RFService.Authorization;
using RFService.Repo;
using RFHttpAction.IServices;
using RFHttpAction.Entities;
using RFUserEmailVerified.Entities;
using RFUserEmailVerified.Exceptions;
using RFUserEmailVerified.IServices;

namespace RFUserEmailVerified.Controllers
{
    [ApiController]
    [Route("v1/my-email")]
    public class MyEmailController(
        IUserEmailVerifiedService userEmailVerifiedService,
        IHttpActionTypeService httpActionTypeService,
        IHttpActionService httpActionService
    ) : ControllerBase
    {
        [HttpPost]
        [Permission("myEmail.create")]
        public async Task<IActionResult> MyEmailPostAsync([FromBody] AddEmailRequest request)
        {
            var userId = HttpContext.Items["UserId"] as long?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            var userEmail = new UserEmailVerified
            {
                UserId = userId.Value,
                Email = request.Email,
            };
            var response = await userEmailVerifiedService.CreateAsync(userEmail);
            return Ok(response);
        }

        [Route("verify")]
        [HttpPost]
        [Permission("myEmail.verify")]
        public async Task<IActionResult> VerifyEmailPostAsync()
        {
            var userId = HttpContext.Items["UserId"] as long?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            var userEmail = await userEmailVerifiedService.GetSingleOrDefaultAsync(new GetOptions { Filters = { { "UserId", userId } } })
                ?? throw new UserDoesNotHaveEmailException();

            if (userEmail.IsVerified)
                throw new UserEmailIsAlreadyVerifiedException();

            var action = await httpActionService.CreateAsync(
                new HttpAction
                {
                    TypeId = await httpActionTypeService.GetIdForNameAsync(
                        "userEmail.verify",
                        creator: name => new HttpActionType
                        {
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
