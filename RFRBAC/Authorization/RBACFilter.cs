using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using RFAuth.Exceptions;
using RFRBAC.IServices;
using RFService.Authorization;

namespace RFRBAC.Authorization
{
    public class RBACFilter(IPermissionService permissionService) : IActionFilter
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

            var permissions = await permissionService.GetAllForUserIdAsync(userId);

            foreach (var permission in permissionAttribute.Permissions) {
                if (permissions.Any(p => p.Name == permission))
                    return;
            }

            context.Result = new StatusCodeResult(403);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {}
    }
}
