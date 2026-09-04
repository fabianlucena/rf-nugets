using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using RFBase.Libs;
using RFEventBus;
using RFPermissions.Attributes;
using RFPermissions.IServices;
using RFRGOBAC.DTO;
using RFRGOBAC.Entities;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;
using RFRGOBACControllers.DTO;
using RFRGOBACControllers.Exceptions;

namespace RFRGOBACControllers.Controllers;

[ApiController]
[Route("v1/organizations")]
public class OrganizationsController(
    IRFRGOBACLoggerService loggerService,
    IOrganizationService organizationService,
    IEventBus eventBus,
    IServiceProvider serviceProvider
) : ControllerBase
{
    [HttpGet]
    [Permission("organizations.get", "organizations.select")]
    public async Task<IActionResult> Get()
    {
        await loggerService.AddInfoGetAsync("Get organizations");

        var organizationOptions = new OrganizationQueryOptions
        {
            IncludeCreatedBy = true,
            IncludeUpdatedBy = true,
            IncludeDeletedBy = true,
        }.BuildFromRequest(Request);

        var permissionService = serviceProvider.GetRequiredService<IPermissionService>();
        if (!permissionService.HasCurrentPermission("organizations.get"))
            organizationOptions.Ids = organizationService.GetCurrentOrganizationsId();

        var organizations = await organizationService.GetListAsync(organizationOptions);
        var response = organizations.Select(organization => new OrganizationResponse(organization));

        return Ok(response);
    }

    [HttpGet("{uuid}")]
    [Permission("organizations.get", "organizations.select")]
    public async Task<IActionResult> Get([FromRoute] Guid uuid)
    {
        await loggerService.AddInfoGetAsync("Get organization", new { uuid });

        var organizationOptions = new OrganizationQueryOptions
        {
            IncludeCreatedBy = true,
            IncludeUpdatedBy = true,
            IncludeDeletedBy = true,
        }.BuildFromRequest(Request);

        var permissionService = serviceProvider.GetRequiredService<IPermissionService>();
        if (!permissionService.HasCurrentPermission("organizations.get"))
            organizationOptions.Ids = organizationService.GetCurrentOrganizationsId();

        organizationOptions.Uuid = uuid;
        var organization = await organizationService.GetSingleOrDefaultAsync(organizationOptions)
            ?? throw new OrganizationWithUuidNotFoundException(uuid);

        return Ok(new OrganizationResponse(organization));
    }

    [HttpGet("current")]
    [Permission("organizations.select")]
    public async Task<IActionResult> GetCurrent()
    {
        await loggerService.AddInfoGetAsync("Get current organization");

        return Ok(organizationService.GetCurrentOrganization());
    }


    [HttpPatch("{uuid}")]
    [Permission("organizations.edit")]
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
    [Permission("organizations.add")]
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
    [Permission("organizations.delete")]
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
    [Permission("organizations.restore")]
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
    }

    [HttpPost("{uuid}/select")]
    [Permission("organizations.select")]
    public async Task<IActionResult> SelectAsync([FromRoute] Guid uuid)
    {
        await loggerService.AddInfoDeleteAsync("Select organization", new { uuid });

        var organizations = organizationService.GetCurrentOrganizations();

        var newOrganizationId = organizations.FirstOrDefault(o => o.Uuid == uuid)?.Id
            ?? throw new OrganizationWithUuidNotFoundException(uuid);

        var sessionService = serviceProvider.GetRequiredService<ISessionService>();
        var session = sessionService.GetCurrentSession()
            ?? throw new NoCurrentSessionException();

        var sessionId = session.Id;

        var sessionOrganizationService = serviceProvider.GetRequiredService<ISessionOrganizationService>();
        var storedOrganizationId = await sessionOrganizationService.GetSingleOrDefaultOrganizationIdBySessionIdAsync(sessionId);

        if (storedOrganizationId <= 0)
        {
            await sessionOrganizationService.CreateAsync(new SessionOrganization
            {
                SessionId = sessionId,
                OrganizationId = newOrganizationId,
            });

            eventBus?.Publish(new Event("SessionUpdated", new DataDictionary {
                { "SessionId", sessionId },
                { "Action", "ChangeOrganization" },
                { "OrganizationId", newOrganizationId },
            }));
        }
        else
        {
            if (storedOrganizationId != newOrganizationId)
            {
                var data = new DataDictionary
                {
                    { "OrganizationId", newOrganizationId }
                };

                var options = new SessionOrganizationQueryOptions
                {
                    SessionId = sessionId
                };

                if (await sessionOrganizationService.UpdateAsync(data, options) <= 0)
                {
                    return BadRequest();
                }

                eventBus?.Publish(new Event("SessionUpdated", new DataDictionary {
                    { "SessionId", sessionId },
                    { "Action", "ChangeOrganization" },
                    { "OrganizationId", newOrganizationId },
                }));
            }
        }

        var orgpDataService = serviceProvider.GetRequiredService<IORGPDataService>();
        var orgpData = await orgpDataService.GetSingleOrDefaultBySession(session)
            ?? throw new NoSessionException();

        return Ok(new ORPGDataResponse(orgpData).Data);
    }
}
