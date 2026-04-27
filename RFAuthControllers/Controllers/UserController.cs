using Microsoft.AspNetCore.Mvc;
using RFBaseEntities.QueryOptions;
using RFBaseIServices.DTO;
using RFBaseIServices.IServices;
using RFPermissionsEntities.Attributes;

namespace RFAuthControllers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        [Permission("user.get")]
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
