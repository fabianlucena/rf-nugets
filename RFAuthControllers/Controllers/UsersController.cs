using Microsoft.AspNetCore.Mvc;
using RFAuth.IServices;
using RFControllers;
using RFIServices.DTO;
using RFIServices.IServices;
using RFIServices.QueryOptions;
using RFPermissions.Attributes;

namespace RFAuthControllers.Controllers;

[ApiController]
[Route("v1/users")]
public class UsersController(
    IRFAuthLoggerService loggerService,
    IUserService userService
) : ControllerBase
{
    [HttpGet("{uuid?}")]
    [Permission("users.get")]
    public async Task<IActionResult> Get([FromRoute] Guid? uuid)
    {
        await loggerService.AddInfoGetAsync("Get users", new { uuid });

        var userOptions = new UserQueryOptions
        {
            IncludeCreatedBy = true,
            IncludeUpdatedBy = true,
            IncludeDeletedBy = true,
        }.BuildFromRequest(Request);

        if (uuid != null)
            userOptions.Uuid = uuid;

        var users = await userService.GetListAsync(userOptions);

        var response = users.Select(user => new UserResponse(user));

        return Ok(new DataRowsResult(response));
    }

    /* [HttpPatch("{uuid}")]
    [Permission("user.edit")]
    public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary request)
    {
        logger.LogInformation("Updating user");

        var data = new DataDictionary(request);
        if (data.ContainsKey("password"))
            data["password"] = "****";
        await loggerService.AddInfoEditAsync("Update user", new { uuid, data });

        request = request.GetPascalized();

        var eventData = new DataDictionary {
            { "Data", request },
            { "Filter", new DataDictionary {{ "Uuid", uuid }}},
        };

        await eventBus.FireAsync("updating", "User", eventData);
        var result = await userService.UpdateForUuidAsync(request, uuid);
        await UpdatePassword(request);
        await eventBus.FireAsync("updated", "User", eventData);

        if (result <= 0)
            return BadRequest();

        logger.LogInformation("User updated");

        return Ok();
    }

    [HttpPost]
    [Permission("user.add")]
    public async Task<IActionResult> PostAsync([FromBody] DataDictionary request)
    {
        logger.LogInformation("Creating user");

        var data = new DataDictionary(request);
        if (data.ContainsKey("password"))
            data["password"] = "****";
        await loggerService.AddInfoAddAsync("Add user", new { data });

        request = request.GetPascalized();
        var eventData = new DataDictionary { { "Data", request } };
        await eventBus.FireAsync("creating", "User", eventData);
        var result = await userService.CreateAsync(request.ToObject<User>());
        await UpdatePassword(request);
        await eventBus.FireAsync("created", "User", eventData);

        if (result == null)
            return BadRequest();

        logger.LogInformation("User created");

        return Ok();
    }

    [HttpDelete("{uuid}")]
    [Permission("user.delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid uuid)
    {
        logger.LogInformation("Deleting user");

        await loggerService.AddInfoDeleteAsync("Delete user", new { uuid });

        var eventData = new DataDictionary { { "Filter", new DataDictionary { { "Uuid", uuid } } } };
        await eventBus.FireAsync("updating", "User", eventData);
        var result = await userService.DeleteForUuidAsync(uuid);
        await eventBus.FireAsync("updated", "User", eventData);

        if (result <= 0)
            return BadRequest();

        logger.LogInformation("User deleted");

        return Ok();
    }

    [HttpPost("restore/{uuid}")]
    [Permission("user.restore")]
    public async Task<IActionResult> RestoreAsync([FromRoute] Guid uuid)
    {
        logger.LogInformation("Restoring user");
        
        await loggerService.AddInfoDeleteAsync("Restore user", new { uuid });

        var eventData = new DataDictionary { { "Filter", new DataDictionary { { "Uuid", uuid } } } };
        await eventBus.FireAsync("restoring", "User", eventData);
        var result = await userService.RestoreForUuidAsync(uuid);
        await eventBus.FireAsync("restored", "User", eventData);

        if (result <= 0)
            return BadRequest();

        logger.LogInformation("User restored");

        return Ok();
    }

    async Task<bool> UpdatePassword(DataDictionary data)
    {
        if (!data.TryGetNotNullString("Username", out var username))
            return false;

        if (!data.TryGetNotNullString("Password", out var password))
            return false;
    
        await passwordService.CreateOrUpdateForUsernameAsync(password, username);

        return true;
    } */
}
