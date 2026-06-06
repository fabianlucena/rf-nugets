using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFHttpAction.Entities;
using RFHttpAction.IServices;
using RFPermissions.Attributes;
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

            var userEmail = await userEmailVerifiedService.GetSingleOrDefaultByEmailAsync(request.Email)
                ?? throw new UserDoesNotHaveEmailException();

            var action = await httpActionService.CreateAsync(
                new HttpAction
                {
                    TypeId = await httpActionTypeService.GetSingleIdByNameOrCreateAsync(
                        "passwordRecovery",
                        createData: async httpActionType => {
                            httpActionType.Title = "PasswordRecovery";
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
