using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFService.Data;
using RFService.Repo;
using RFService.Authorization;
using RFRBAC.IServices;
using RFRBAC.DTO;
using RFRBAC.Entities;
using RFService.Libs;

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

            var query = HttpContext.Request.Query.GetPascalized();
            var options = GetOptions.CreateFromQuery(query);
            if (uuid != null)
                options.Filters["uuid"] = uuid;

            if (query.TryGetBool("IsSelectable", out bool isSelectable))
                options.Filters["IsSelectable"] = isSelectable;
            
            var roleList = await roleService.GetListAsync(options);
            var roleAttributesList = roleList.Select(mapper.Map<Role, RoleAttributes>);

            roleAttributesList = await roleService.DecorateListAsync(
                roleAttributesList,
                "RoleAttributes",
                (row, value) => row.Attributes = value,
                eventType: "Result"
            );

            var response = roleAttributesList.Select(mapper.Map<RoleAttributes, RoleResponse>);

            return Ok(new DataRowsResult(response));
        }
    }
}
