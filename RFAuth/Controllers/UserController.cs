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
using RFService.Services;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/user")]
    public class UserController(
        ILogger<UserController> logger,
        IUserService userService,
        IMapper mapper,
        IEventBus eventBus
    ) : ControllerBase
    {
        [HttpGet("{uuid?}")]
        [Permission("user.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting users");

            var options = new GetOptions { Include = { { "Type", new GetOptions() } } };
            if (uuid != null)
            {
                options.Filters["uuid"] = uuid;
            }

            var userList = await userService.GetListAsync(options);
            var userAttributesList = userList.Select(mapper.Map<User, UserAttributes>);

            userAttributesList = await userService.DecorateAsync(
                userAttributesList,
                "UserAttributes",
                (row, value) => {
                    if (row.Attributes == null)
                        row.Attributes = value;
                    else
                        row.Attributes = row.Attributes
                            .Concat(value)
                            .ToDictionary(pair => pair.Key, pair => pair.Value);
                },
                eventType: "Result"
            );

            var response = userAttributesList.Select(mapper.Map<UserAttributes, UserResponse>);

            return Ok(new DataRowsResult(response));
        }

        [HttpPatch("{uuid}")]
        [Permission("user.edit")]
        public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] Dictionary<string, object?> data)
        {
            logger.LogInformation("Updating users");

            var eventData = new EventData {
                Data = data,
                Filter = new Dictionary<string, object?> {
                    { "Uuid", uuid }
                }
            };
            await eventBus.FireAsync("updating", "User", eventData);
            var result = await userService.UpdateForUuidAsync(data, uuid);
            await eventBus.FireAsync("updated", "User", eventData);

            if (result <= 0)
                return BadRequest();

            return Ok();
        }
    }
}
