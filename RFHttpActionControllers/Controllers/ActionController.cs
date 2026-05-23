using Microsoft.AspNetCore.Mvc;
using RFHttpAction.IServices;

namespace RFHttpActionControllers.Controllers
{
    [ApiController]
    [Route("v1/action")]
    public class ActionController(
        IHttpActionService httpActionService,
        IHttpActionTypeService httpActionTypeService,
        IHttpActionListeners httpActionListeners
    ) : ControllerBase
    {
        [HttpPost("{token}")]
        public async Task<IActionResult> PostAsync([FromRoute] string token)
        {
            var httpAction = await httpActionService.GetSingleByTokenAsync(token);
            var httpActionType = await httpActionTypeService.GetSingleByIdAsync(httpAction.TypeId);
            var listeners = httpActionListeners.GetListeners(httpActionType.Name);
            if (listeners != null)
            {
                await httpActionService.DeleteByIdAsync(httpAction.Id);
                foreach (var listener in listeners)
                {
                    await listener(httpAction);
                }
            }

            return Ok();
        }
    }
}
