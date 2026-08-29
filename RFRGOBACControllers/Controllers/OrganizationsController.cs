using Microsoft.AspNetCore.Mvc;
using RFBase.Libs;
using RFControllers;
using RFEventBus;
using RFPermissions.Attributes;
using RFRGOBAC.Entities;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;
using RFRGOBACControllers.DTO;
using RFRGOBACControllers.IServices;

namespace RFRGOBACControllers.Controllers;

[ApiController]
[Route("v1/organizations")]
public class OrganizationsController(
    IOrganizationService organizationService,
    IRFRGOBACControllersLoggerService loggerService,
    IEventBus eventBus
) : ControllerBase
{
    [HttpGet("{uuid?}")]
    [Permission("organizations.get")]
    public async Task<IActionResult> Get([FromRoute] Guid? uuid)
    {
        await loggerService.AddInfoGetAsync("Get organizations", new { uuid });

        var organizationOptions = new OrganizationQueryOptions
        {
            IncludeCreatedBy = true,
            IncludeUpdatedBy = true,
            IncludeDeletedBy = true,
        }.BuildFromRequest(Request);

        if (uuid != null)
            organizationOptions.Uuid = uuid;

        var organizations = await organizationService.GetListAsync(organizationOptions);

        var response = organizations.Select(organization => new OrganizationResponse(organization));

        return Ok(new DataRowsResult(response));
    }

    [HttpPatch("{uuid}")]
    [Permission("organizations.edit")]
    public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary request)
    {
        await loggerService.AddInfoEditAsync("Update organization", new { uuid, request });

        var data = request.GetPascalized();
        var result = await organizationService.UpdateByUuidAsync(uuid, data);

        _ = eventBus.Publish(new Event("OrganizationUpdated", new DataDictionary {
            { "Data", data },
            { "Filter", new DataDictionary {{ "Uuid", uuid }}},
        }));

        if (result <= 0)
            return BadRequest();

        return Ok();
    }

    [HttpPost]
    [Permission("organizations.add")]
    public async Task<IActionResult> PostAsync([FromBody] DataDictionary request)
    {
        await loggerService.AddInfoAddAsync("Add organization", new { request });

        var data = request.GetPascalized();
        var result = await organizationService.CreateAsync(request.ToObject<Organization>());

        _ = eventBus.Publish(new Event("OrganizationCreated", new DataDictionary {
            { "Data", data }
        }));

        if (result == null)
            return BadRequest();

        return Ok();
    }

    [HttpDelete("{uuid}")]
    [Permission("organizations.delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid uuid)
    {
        await loggerService.AddInfoDeleteAsync("Delete organization", new { uuid });

        var result = await organizationService.DeleteByUuidAsync(uuid);

        _ = eventBus.Publish(new Event("OrganizationDeleted", new DataDictionary {
            { "Filter", new DataDictionary { { "Uuid", uuid } } }
        }));

        if (result <= 0)
            return BadRequest();

        return Ok();
    }

    [HttpPost("restore/{uuid}")]
    [Permission("organizations.restore")]
    public async Task<IActionResult> RestoreAsync([FromRoute] Guid uuid)
    {
        await loggerService.AddInfoDeleteAsync("Restore organization", new { uuid });

        var result = await organizationService.RestoreByUuidAsync(uuid);

        _ = eventBus.Publish(new Event("OrganizationRestored", new DataDictionary {
            { "Filter", new DataDictionary { { "Uuid", uuid } } }
        }));

        if (result <= 0)
            return BadRequest();

        return Ok();
    }
}
