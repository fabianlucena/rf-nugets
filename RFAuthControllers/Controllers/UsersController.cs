using Microsoft.AspNetCore.Mvc;
using RFBaseEntities.QueryOptions;
using RFBaseIServices.DTO;
using RFBaseIServices.IServices;
using RFPermissionsEntities.Attributes;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        [Permission("users.get")]
        public async Task<IActionResult> Get()
        {
            var users = await userService.GetListAsync(new UserQueryOptions
            {
                IncludeCreatedBy = true,
                IncludeUpdatedBy = true,
                IncludeDeletedBy = true,
            });

            var response = users.Select(user => new UserResponse(user));

            return Ok(response);
        }
    }
}
