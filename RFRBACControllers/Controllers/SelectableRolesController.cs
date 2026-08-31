using Microsoft.AspNetCore.Mvc;
using RFPermissions.Attributes;
using RFRBAC.DTO;
using RFRBAC.Exceptions;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;

namespace RFRBACControllers.Controllers;

[ApiController]
[Route("v1/selectable-roles")]
public class SelectableRolesController(
    IRFRBACLoggerService loggerService,
    IRoleService roleService
) : ControllerBase
{
    [HttpGet("{uuid?}")]
    [Permission("selectableRole.get")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
    {
        await loggerService.AddInfoGetAsync("Get roles", new { uuid });

        var roleOptions = new RoleQueryOptions
        {
            IsSelectable = true,
        }.BuildFromRequest(Request);

        if (uuid != null)
        {
            roleOptions.Uuid = uuid;
            var organization = await roleService.GetSingleOrDefaultAsync(roleOptions)
                ?? throw new RoleWithUuidNotFoundException(uuid.Value);

            return Ok(new SelectableRoleResponse(organization));
        }

        var organizations = await roleService.GetListAsync(roleOptions);
        var response = organizations.Select(organization => new SelectableRoleResponse(organization));

        return Ok(response);
    }
}
