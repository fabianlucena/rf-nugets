using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using RFService.Authorization;

namespace RFRBAC.Authorization
{
    public class RBACFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
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

            // context.Result = new ForbidResult(); // Bloquear el acceso si no cumple con la edad mínima
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No se necesita implementar nada aquí para este ejemplo
        }
    }
}
