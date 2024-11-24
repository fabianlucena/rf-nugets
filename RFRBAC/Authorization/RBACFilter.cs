using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.Authorization;

namespace RFRBAC.Authorization
{
    public class RBACFilter(
        IUserRoleService userRoleService,
        IPermissionService permissionService
    ) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
            {
                await next();
                return;
            }

            // Obtener el atributo aplicado
            var permissionAttribute = (PermissionAttribute?)Attribute.GetCustomAttribute(
                controllerActionDescriptor.MethodInfo,
                typeof(PermissionAttribute)
            );

            if (permissionAttribute == null)
            {
                await next();
                return;
            }

            var httpContext = context.HttpContext;
            var userIdText = httpContext.Items["UserId"];
            var userId = Convert.ToInt64(userIdText);
            if (userId <= 0)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            var allRoles = await userRoleService.GetAllRolesForUserIdAsync(userId);
            if (allRoles.Any(i => i.Name == "admin"))
            {
                await next();
                return;
            }

            var permissions = await permissionService.GetAllForUserIdAsync(userId);
            foreach (var permission in permissionAttribute.Permissions) {
                if (permissions.Any(p => p.Name == permission))
                {
                    await next();
                    return;
                }
            }

            context.Result = new StatusCodeResult(403);
        }
    }
}
