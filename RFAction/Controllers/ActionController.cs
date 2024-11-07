using Microsoft.AspNetCore.Mvc;
using RFHttpAction.IServices;

namespace RFHttpAction.Controllers
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
            var httpAction = await httpActionService.GetSingleForTokenAsync(token);
            var httpActionType = await httpActionTypeService.GetForIdAsync(httpAction.TypeId);
            var listeners = httpActionListeners.GetListeners(httpActionType.Name);
            if (listeners != null)
            {
                await httpActionService.DeleteForIdAsync(httpAction.Id);
                foreach (var listener in listeners)
                {
                    await listener(httpAction);
                }
            }

            return Ok();
        }
    }
}
