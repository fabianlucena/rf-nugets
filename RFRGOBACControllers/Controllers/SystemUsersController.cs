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
public class SystemUsersController(
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
        await loggerService.AddInfoGetAsync("Get system users", new { uuid });

        var userOptions = new SystemUserQueryOptions
        {
            IncludeCreatedBy = true,
            IncludeUpdatedBy = true,
            IncludeDeletedBy = true,
        }.UpdateFromRequest(Request);

        if (uuid != null)
        {
            userOptions.Uuid = uuid;
            var user = await systemUserService.GetSingleOrDefaultAsync(userOptions)
                ?? throw new UserWithUuidNotFoundException(uuid.Value);

            user = await systemUserService.Translate(user);

            return Ok(new SystemUserResponse(user));
        }

        var users = await systemUserService.GetListAsync(userOptions);
        users = await systemUserService.Translate(users);
        var response = users.Select(user => new SystemUserResponse(user));

        return Ok(response);
    }

    [HttpPost]
    [Permission("systemUsers.add")]
    public async Task<IActionResult> PostAsync([FromBody] SystemUserRequest request)
    {
        await loggerService.AddInfoAddAsync("Add system user", new { request });

        var result = await systemUserService.CreateAsync(await request.ToSystemUser(serviceProvider));

        _ = eventBus.Publish(new Event("SystemUserCreated", new DataDictionary {
            { "Data", request }
        }));

        if (result == null)
            return BadRequest();

        return NoContent();
    }

    [HttpPatch("{uuid}")]
    [Permission("systemUsers.update")]
    public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary request)
    {
        await loggerService.AddInfoEditAsync("Update system user", new { uuid, request });

        var userOptions = new SystemUserQueryOptions
        {
            IncludeInactive = true
        }.UpdateFromRequest(Request);

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
        await loggerService.AddInfoDeleteAsync("Delete system user", new { uuid });

        var userOptions = new SystemUserQueryOptions
        {
            IncludeInactive = true
        }.UpdateFromRequest(Request);

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
        await loggerService.AddInfoDeleteAsync("Restore system user", new { uuid });

        var userOptions = new SystemUserQueryOptions
        {
            IncludeDeleted = true,
            IncludeInactive = true,
        }.UpdateFromRequest(Request);

        var result = await systemUserService.RestoreByUuidAsync(uuid, userOptions);

        _ = eventBus.Publish(new Event("SystemUserRestored", new DataDictionary {
            { "Filter", new DataDictionary { { "Uuid", uuid } } }
        }));

        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpGet("selectable-roles/{uuid?}")]
    [Permission("selectableRole.get")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
    {
        await loggerService.AddInfoGetAsync("Get roles", new { uuid });

        var roleService = serviceProvider.GetRequiredService<IRoleService>();

        var roleOptions = new RoleQueryOptions
        {
            IsSelectable = true,
        }.UpdateFromRequest(Request);

        if (uuid != null)
        {
            roleOptions.Uuid = uuid;
            var role = await roleService.GetSingleOrDefaultAsync(roleOptions)
                ?? throw new RoleWithUuidNotFoundException(uuid.Value);

            return Ok(new RoleResponse(await roleService.Translate(role)));
        }

        var roles = await roleService.GetListAsync(roleOptions);
        var response = (await roleService.Translate(roles)).Select(role => new RoleResponse(role));

        return Ok(response);
    }
}
