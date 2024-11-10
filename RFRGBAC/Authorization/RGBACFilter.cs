using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using RFAuth.Exceptions;
using RFRBAC.IServices;
using RFRGBAC.IServices;
using RFService.Authorization;

namespace RFRGBAC.Authorization
{
    public class RGBACFilter(
        IPermissionService permissionService,
        IUserGroupService userGroupService
    ) : IActionFilter
    {
        public async void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
                return;

            // Obtener el atributo aplicado
            var permissionAttribute = (PermissionAttribute?)Attribute.GetCustomAttribute(
                controllerActionDescriptor.MethodInfo,
                typeof(PermissionAttribute)
            );

            if (permissionAttribute == null)
                return;

            var httpContext = context.HttpContext;
            var userIdText = httpContext.Items["UserId"];
            var userId = Convert.ToInt64(userIdText);
            if (userId <= 0)
                throw new NoAuthorizationHeaderException();

            var allGroupsId = await userGroupService.GetAllGroupsIdForUserIdAsync(userId);
            var permissions = await permissionService.GetAllForUsersIdAsync(allGroupsId);

            foreach (var permission in permissionAttribute.Permissions)
            {
                if (permissions.Any(p => p.Name == permission))
                    return;
            }

            context.Result = new StatusCodeResult(403);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}
