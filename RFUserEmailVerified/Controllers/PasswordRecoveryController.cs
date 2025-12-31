using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFHttpAction.Entities;
using RFHttpAction.IServices;
using RFService.Authorization;
using RFService.Repo;
using RFUserEmailVerified.DTO;
using RFUserEmailVerified.Exceptions;
using RFUserEmailVerified.IServices;

namespace RFUserEmailVerified.Controllers
{
    [ApiController]
    [Route("v1/password-recovery")]
    public class PasswordRecoveryController(
        ILogger<PasswordRecoveryController> logger,
        IUserEmailVerifiedService userEmailVerifiedService,
        IHttpActionTypeService httpActionTypeService,
        IHttpActionService httpActionService
    ) : ControllerBase
    {
        [HttpPost]
        [Permission("passwordRecovery.create")]
        public virtual async Task<IActionResult> PasswordRecoveryPostAsync([FromBody] PasswordRecoveryRequest request)
        {
            logger.LogInformation("Recovering password");

            var userEmail = await userEmailVerifiedService.GetSingleOrDefaultAsync(new QueryOptions { Filters = { { "Email", request.Email } } })
                ?? throw new UserDoesNotHaveEmailException();

            var action = await httpActionService.CreateAsync(
                new HttpAction
                {
                    TypeId = await httpActionTypeService.GetSingleIdForNameAsync(
                        "passwordRecovery",
                        creator: name => new HttpActionType
                        {
                            Name = name,
                            Title = "PasswordRecovery",
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
