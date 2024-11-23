using AutoMapper;
using RFAuth.Entities;
using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFService.Data;
using RFService.Repo;
using RFService.Authorization;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/user")]
    public class UserController(
        ILogger<UserController> logger,
        IUserService userService,
        IMapper mapper
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
                (row, value) => row.Attributes = value,
                destiny: "Result"
            );

            var response = userAttributesList.Select(mapper.Map<UserAttributes, UserResponse>);

            return Ok(new DataRowsResult(response));
        }

        [HttpPatch("{uuid}")]
        [Permission("user.edit")]
        public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] Dictionary<string, object?> data)
        {
            logger.LogInformation("Updating users");

            var result = await userService.UpdateForUuidAsync(data, uuid);

            if (result > 0)
            {
                return Ok();
            } else
            {
                return BadRequest();
            }
        }
    }
}
