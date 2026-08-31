using Microsoft.AspNetCore.Mvc;
using RFBase.Libs;
using RFEventBus;
using RFIServices.IServices;
using RFIServices.QueryOptions;
using RFPermissions.Attributes;
using RFRGOBAC.Entities;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;
using RFRGOBAC.Services;
using RFRGOBACControllers.DTO;
using RFRGOBACControllers.Exceptions;

namespace RFRGOBACControllers.Controllers;

[ApiController]
[Route("v1/system-users")]
public class OrganizationUsersController(
    ISystemUserService systemUserService,
    IRFRGOBACLoggerService loggerService //,
    // IEventBus eventBus
) : ControllerBase
{
    [HttpGet("{uuid?}")]
    [Permission("systemUsers.get")]
    public async Task<IActionResult> Get([FromRoute] Guid? uuid)
    {
        await loggerService.AddInfoGetAsync("Get organizations", new { uuid });

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

    /*[HttpPatch("{uuid}")]
    [Permission("systemUsers.edit")]
    public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary request)
    {
        await loggerService.AddInfoEditAsync("Update organization", new { uuid, request });

        var organizationOptions = new OrganizationQueryOptions
        {
            IncludeInactive = true
        }.BuildFromRequest(Request);

        var data = request.GetPascalized();
        var result = await organizationService.UpdateByUuidAsync(uuid, data, organizationOptions);

        _ = eventBus.Publish(new Event("OrganizationUpdated", new DataDictionary {
            { "Data", data },
            { "Filter", new DataDictionary {{ "Uuid", uuid }}},
        }));

        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpPost]
    [Permission("systemUsers.add")]
    public async Task<IActionResult> PostAsync([FromBody] DataDictionary request)
    {
        await loggerService.AddInfoAddAsync("Add organization", new { request });

        var data = request.GetPascalized();
        var result = await organizationService.CreateAsync(data.ToObject<Organization>());

        _ = eventBus.Publish(new Event("OrganizationCreated", new DataDictionary {
            { "Data", data }
        }));

        if (result == null)
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{uuid}")]
    [Permission("systemUsers.delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid uuid)
    {
        await loggerService.AddInfoDeleteAsync("Delete organization", new { uuid });

        var organizationOptions = new OrganizationQueryOptions
        {
            IncludeInactive = true
        }.BuildFromRequest(Request);

        var result = await organizationService.DeleteByUuidAsync(uuid, organizationOptions);

        _ = eventBus.Publish(new Event("OrganizationDeleted", new DataDictionary {
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
        await loggerService.AddInfoDeleteAsync("Restore organization", new { uuid });

        var organizationOptions = new OrganizationQueryOptions
        {
            IncludeDeleted = true,
            IncludeInactive = true,
        }.BuildFromRequest(Request);

        var result = await organizationService.RestoreByUuidAsync(uuid, organizationOptions);

        _ = eventBus.Publish(new Event("OrganizationRestored", new DataDictionary {
            { "Filter", new DataDictionary { { "Uuid", uuid } } }
        }));

        if (result <= 0)
            return BadRequest();

        return NoContent();
    } */
}
