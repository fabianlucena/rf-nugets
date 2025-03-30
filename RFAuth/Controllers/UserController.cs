using AutoMapper;
using RFAuth.Entities;
using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFService.Data;
using RFService.Repo;
using RFService.Authorization;
using RFService.IServices;
using RFService.Libs;
using RFLogger.IServices;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/user")]
    public class UserController(
        ILogger<UserController> logger,
        IUserService userService,
        IUserTypeService userTypeService,
        IPasswordService passwordService,
        ILoggerService loggerService,
        IMapper mapper,
        IEventBus eventBus
    ) : ControllerBase
    {
        [HttpGet("{uuid?}")]
        [Permission("user.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting users");

            await loggerService.AddInfoGetAsync("Get users", new { uuid });

            var options = GetOptions.CreateFromQuery(HttpContext);
            if (!options.HasColumnFilter("TypeId"))
            {
                var userTypeIdUser = await userTypeService.GetSingleIdForNameAsync("user");
                options.AddFilter("TypeId", userTypeIdUser);
            }

            if (uuid != null)
                options.AddFilter("Uuid", uuid);

            var userList = await userService.GetListAsync(options);
            var userAttributesList = userList.Select(mapper.Map<User, UserAttributes>);

            userAttributesList = await userService.DecorateListAsync(
                userAttributesList,
                "UserAttributes",
                (row, value) => {
                    if (row.Attributes == null)
                        row.Attributes = value;
                    else
                        row.Attributes = row.Attributes
                            .Concat(value)
                            .ToDataDictionary();
                },
                eventType: "Result"
            );

            var response = userAttributesList.Select(mapper.Map<UserAttributes, UserResponse>);

            logger.LogInformation("Users getted");

            return Ok(new DataRowsResult(response));
        }

        [HttpPatch("{uuid}")]
        [Permission("user.edit")]
        public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary request)
        {
            logger.LogInformation("Updating user");

            var data = new DataDictionary(request);
            if (data.ContainsKey("password"))
                data["password"] = "****";
            await loggerService.AddInfoEditAsync("Update user", new { uuid, data });

            request = request.GetPascalized();

            var eventData = new EventData {
                Data = request,
                Filter = new DataDictionary {
                    { "Uuid", uuid }
                }
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
            var eventData = new EventData { Data = request };
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

            var eventData = new EventData
            {
                Filter = new DataDictionary {
                    { "Uuid", uuid }
                }
            };
            
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

            var eventData = new EventData { Filter = new DataDictionary { { "Uuid", uuid } } };

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
        }
    }
}
