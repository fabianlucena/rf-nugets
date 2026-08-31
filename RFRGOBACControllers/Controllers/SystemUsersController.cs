using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RFBase.Libs;
using RFEventBus;
using RFPermissions.Attributes;
using RFRBAC.DTO;
using RFRBAC.Exceptions;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFRGOBAC.DTO;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;
using RFRGOBACControllers.DTO;
using RFRGOBACControllers.Exceptions;

namespace RFRGOBACControllers.Controllers;

[ApiController]
[Route("v1/system-users")]
public class OrganizationUsersController(
    ISystemUserService systemUserService,
    IRFRGOBACLoggerService loggerService,
    IEventBus eventBus,
    IServiceProvider serviceProvider
) : ControllerBase
{
    [HttpGet("{uuid?}")]
    [Permission("systemUsers.get")]
    public async Task<IActionResult> Get([FromRoute] Guid? uuid)
    {
        await loggerService.AddInfoGetAsync("Get users", new { uuid });

        var userOptions = new OrganizationUserQueryOptions
        {
            IncludeCreatedBy = true,
            IncludeUpdatedBy = true,
            IncludeDeletedBy = true,
            IncludeType = true,
        }.BuildFromRequest(Request);

        if (uuid != null)
        {
            userOptions.Uuid = uuid;
            var user = await systemUserService.GetSingleOrDefaultAsync(userOptions)
                ?? throw new UserWithUuidNotFoundException(uuid.Value);

            return Ok(new OrganizationUserResponse(user));
        }

        var users = await systemUserService.GetListAsync(userOptions);
        var response = users.Select(user => new OrganizationUserResponse(user));

        return Ok(response);
    }

    [HttpPost]
    [Permission("systemUsers.add")]
    public async Task<IActionResult> PostAsync([FromBody] DataDictionary request)
    {
        await loggerService.AddInfoAddAsync("Add user", new { request });

        var data = request.GetPascalized();
        var result = await systemUserService.CreateAsync(data.ToObject<OrganizationUser>());

        _ = eventBus.Publish(new Event("SystemUserCreated", new DataDictionary {
            { "Data", data }
        }));

        if (result == null)
            return BadRequest();

        return NoContent();
    }

    [HttpPatch("{uuid}")]
    [Permission("systemUsers.edit")]
    public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary request)
    {
        await loggerService.AddInfoEditAsync("Update user", new { uuid, request });

        var userOptions = new OrganizationUserQueryOptions
        {
            IncludeInactive = true
        }.BuildFromRequest(Request);

        var data = request.GetPascalized();
        var result = await systemUserService.UpdateByUuidAsync(uuid, data, userOptions);

        _ = eventBus.Publish(new Event("SystemUserUpdated", new DataDictionary {
            { "Data", data },
            { "Filter", new DataDictionary {{ "Uuid", uuid }}},
        }));

        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{uuid}")]
    [Permission("systemUsers.delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid uuid)
    {
        await loggerService.AddInfoDeleteAsync("Delete user", new { uuid });

        var userOptions = new OrganizationUserQueryOptions
        {
            IncludeInactive = true
        }.BuildFromRequest(Request);

        var result = await systemUserService.DeleteByUuidAsync(uuid, userOptions);

        _ = eventBus.Publish(new Event("SystemUserDeleted", new DataDictionary {
            { "Filter", new DataDictionary { { "Uuid", uuid } } }
        }));

        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpPost("{uuid}/restore")]
    [Permission("systemUsers.restore")]
    public async Task<IActionResult> RestoreAsync([FromRoute] Guid uuid)
    {
        await loggerService.AddInfoDeleteAsync("Restore user", new { uuid });

        var userOptions = new OrganizationUserQueryOptions
        {
            IncludeDeleted = true,
            IncludeInactive = true,
        }.BuildFromRequest(Request);

        var result = await systemUserService.RestoreByUuidAsync(uuid, userOptions);

        _ = eventBus.Publish(new Event("SystemUserRestored", new DataDictionary {
            { "Filter", new DataDictionary { { "Uuid", uuid } } }
        }));

        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpGet("{uuid?}")]
    [Permission("selectableRole.get")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
    {
        await loggerService.AddInfoGetAsync("Get roles", new { uuid });

        var roleService = serviceProvider.GetRequiredService<IRoleService>();

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
