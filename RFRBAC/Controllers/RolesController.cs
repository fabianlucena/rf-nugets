using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFService.Data;
using RFService.Repo;
using RFService.Authorization;
using RFRBAC.IServices;
using RFRBAC.DTO;
using RFRBAC.Entities;

namespace RFRBAC.Controllers
{
    [ApiController]
    [Route("v1/role")]
    public class RolesController(
        ILogger<RolesController> logger,
        IRoleService roleService,
        IMapper mapper
    ) : ControllerBase
    {
        [HttpGet("{uuid?}")]
        [Permission("role.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting roles");

            var options = new GetOptions();
            if (uuid != null)
            {
                options.Filters["uuid"] = uuid;
            }

            var roleList = await roleService.GetListAsync(options);
            var roleAttributesList = roleList.Select(mapper.Map<Role, RoleAttributes>);

            roleAttributesList = await roleService.DecorateAsync(
                roleAttributesList,
                "RoleAttributes",
                (row, value) => row.Attributes = value,
                eventName: "Result"
            );

            var response = roleAttributesList.Select(mapper.Map<RoleAttributes, RoleResponse>);

            return Ok(new DataRowsResult(response));
        }
    }
}
