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
[Route("v1/organization-users")]
public class OrganizationUsersController(
    IOrganizationUserService organizationUserService,
    IRFRGOBACLoggerService loggerService,
    IOrganizationService organizationService,
    IEventBus eventBus,
    IServiceProvider serviceProvider
) : ControllerBase
{
    [HttpGet("{uuid?}")]
    [Permission("organizationUsers.get")]
    public async Task<IActionResult> Get([FromRoute] Guid? uuid)
    {
        await loggerService.AddInfoGetAsync("Get organization users", new { uuid });

        var organizationId = organizationService.GetCurrentOrganizationId()
            ?? throw new NoCurrentOrganizationException();

        var userOptions = new OrganizationUserQueryOptions
        {
            IncludeCreatedBy = true,
            IncludeUpdatedBy = true,
            IncludeDeletedBy = true,
            OrganizationId = organizationId
        }.BuildFromRequest(Request);

        if (userOptions.OrganizationId != organizationId)
            throw new InvalidOrganizationOrganizationException();

        if (uuid != null)
        {
            userOptions.Uuid = uuid;
            var user = await organizationUserService.GetSingleOrDefaultAsync(userOptions)
                ?? throw new UserWithUuidNotFoundException(uuid.Value);

            user = await organizationUserService.Translate(user);

            return Ok(new OrganizationUserResponse(user));
        }

        var users = await organizationUserService.GetListAsync(userOptions);
        users = await organizationUserService.Translate(users);
        var response = users.Select(user => new OrganizationUserResponse(user));

        return Ok(response);
    }

    [HttpPost]
    [Permission("organizationUsers.add")]
    public async Task<IActionResult> PostAsync([FromBody] OrganizationUserRequest request)
    {
        await loggerService.AddInfoAddAsync("Add organization user", new { request });

        var organizationId = organizationService.GetCurrentOrganizationId()
            ?? throw new NoCurrentOrganizationException();

        var result = await organizationUserService.CreateAsync(await request.ToOrganizationUser(serviceProvider));

        _ = eventBus.Publish(new Event("OrganizationUserCreated", new DataDictionary {
            { "Data", request }
        }));

        if (result == null)
            return BadRequest();

        return NoContent();
    }

    [HttpPatch("{uuid}")]
    [Permission("organizationUsers.update")]
    public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary request)
    {
        await loggerService.AddInfoEditAsync("Update organization user", new { uuid, request });

        var organizationId = organizationService.GetCurrentOrganizationId()
            ?? throw new NoCurrentOrganizationException();

        var user = await organizationUserService.GetSingleOrDefaultAsync(new OrganizationUserQueryOptions
        {
            IncludeCanEdit = true,
            IncludeInactive = true,
            IncludeDeleted = true,
            OrganizationId = organizationId,
            Uuid = uuid,
        }.BuildFromRequest(Request)) ?? throw new UserWithUuidNotFoundException(uuid);

        if (user.CanEdit == false)
            throw new UserWithUuidCannotBeEditedException(uuid);

        if (user.DeletedAt != null)
            throw new UserWithUuidIsDeletedException(uuid);

        var userOptions = new OrganizationUserQueryOptions
        {
            IncludeInactive = true,
        }.BuildFromRequest(Request);

        var data = request.GetPascalized();
        var result = await organizationUserService.UpdateByUuidAsync(uuid, data, userOptions);

        _ = eventBus.Publish(new Event("OrganizationUserUpdated", new DataDictionary {
            { "Data", data },
            { "Filter", new DataDictionary {{ "Uuid", uuid }}},
        }));

        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{uuid}")]
    [Permission("organizationUsers.delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid uuid)
    {
        await loggerService.AddInfoDeleteAsync("Delete organization user", new { uuid });

        var organizationId = organizationService.GetCurrentOrganizationId()
            ?? throw new NoCurrentOrganizationException();

        var user = await organizationUserService.GetSingleOrDefaultAsync(new OrganizationUserQueryOptions
        {
            IncludeCanEdit = true,
            IncludeInactive = true,
            IncludeDeleted = true,
            OrganizationId = organizationId,
            Uuid = uuid,
        }.BuildFromRequest(Request)) ?? throw new UserWithUuidNotFoundException(uuid);

        if (user.CanEdit == false)
            throw new UserWithUuidCannotBeEditedException(uuid);

        var userOptions = new OrganizationUserQueryOptions
        {
            IncludeInactive = true
        }.BuildFromRequest(Request);

        var result = await organizationUserService.DeleteByUuidAsync(uuid, userOptions);

        _ = eventBus.Publish(new Event("OrganizationUserDeleted", new DataDictionary {
            { "Filter", new DataDictionary { { "Uuid", uuid } } }
        }));

        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpPost("{uuid}/restore")]
    [Permission("organizationUsers.restore")]
    public async Task<IActionResult> RestoreAsync([FromRoute] Guid uuid)
    {
        await loggerService.AddInfoDeleteAsync("Restore organization user", new { uuid });

        var organizationId = organizationService.GetCurrentOrganizationId()
            ?? throw new NoCurrentOrganizationException();

        var user = await organizationUserService.GetSingleOrDefaultAsync(new OrganizationUserQueryOptions
        {
            IncludeCanEdit = true,
            IncludeInactive = true,
            IncludeDeleted = true,
            OrganizationId = organizationId,
            Uuid = uuid,
        }.BuildFromRequest(Request)) ?? throw new UserWithUuidNotFoundException(uuid);

        if (user.CanEdit == false)
            throw new UserWithUuidCannotBeEditedException(uuid);

        var userOptions = new OrganizationUserQueryOptions
        {
            IncludeDeleted = true,
            IncludeInactive = true,
        }.BuildFromRequest(Request);

        var result = await organizationUserService.RestoreByUuidAsync(uuid, userOptions);

        _ = eventBus.Publish(new Event("OrganizationUserRestored", new DataDictionary {
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
        }.BuildFromRequest(Request);

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
