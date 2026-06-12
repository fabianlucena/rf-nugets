using Microsoft.AspNetCore.Mvc;
using RFHttpAction.Entities;
using RFUserEmailVerified.Exceptions;
using RFHttpAction.IServices;
using RFUserEmailVerified.IServices;
using RFUserEmailVerified.Entities;
using RFUserEmailVerified.DTO;
using RFPermissions.Attributes;
using RFAuthControllers.Exceptions;

namespace RFUserEmailVerified.Controllers
{
    [ApiController]
    [Route("v1/my-email")]
    public class MyEmailVerifiedController(
        IUserEmailVerifiedService userEmailVerifiedService,
        IHttpActionTypeService httpActionTypeService,
        IHttpActionService httpActionService
    )
        : ControllerBase
    {
        [HttpPost]
        [Permission("myEmail.create")]
        public async Task<IActionResult> MyEmailVerifiedPostAsync([FromBody] AddEmailRequest request)
        {
            var userId = HttpContext.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            var userEmail = new UserEmailVerified
            {
                UserId = userId.Value,
                Email = request.Email,
            };
            if ((await userEmailVerifiedService.CreateAsync(userEmail)) == null)
                throw new ErrorToCreateEmailException();

            return Ok();
        }

        [Route("verify")]
        [HttpPost]
        [Permission("myEmail.verify")]
        public async Task<IActionResult> VerifyEmailPostAsync()
        {
            var userId = HttpContext.Items["UserId"] as long?
                ?? throw new NoAuthorizationHeaderException();

            if (userId == 0)
                throw new NoAuthorizationHeaderException();

            var userEmail = await userEmailVerifiedService.GetSingleOrDefaultByUserIdAsync(userId)
                ?? throw new UserDoesNotHaveEmailException();

            if (userEmail.IsVerified)
                throw new UserEmailIsAlreadyVerifiedException();

            var action = await httpActionService.CreateAsync(
                new HttpAction
                {
                    TypeId = await httpActionTypeService.GetIdOrCreateByNameAsync(
                        "userEmail.verify",
                        createFactory: async httpActionType => {
                            httpActionType.Title = "UserEmail Verify";
                            return httpActionType;
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
