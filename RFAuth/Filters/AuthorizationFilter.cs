using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using RFPermissions.Attributes;

namespace RFAuth.Filters
{
    public class AuthorizationFilter() : IAsyncActionFilter
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

            if (httpContext.Items.ContainsKey("RolesName")
                && httpContext.Items["RolesName"] is IEnumerable<string> roles
                && roles.Any(i => i == "admin"))
            {
                await next();
                return;
            }

            if (httpContext.Items.ContainsKey("PermissionsName")
                && httpContext.Items["PermissionsName"] is IEnumerable<string> permissions)
            {
                if (permissionAttribute.Permissions.Any(permission => permissions.Any(p => p == permission)))
                {
                    await next();
                    return;
                }
            }

            context.Result = new StatusCodeResult(403);
        }
    }
}
