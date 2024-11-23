using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFRBAC.DTO;
using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IServices;

namespace RFRBAC
{
    public static class Setup
    {
        public static void ConfigureRFRBAC(IServiceProvider provider)
        {
            var roleService = provider.GetRequiredService<IRoleService>();
            var mapper = provider.GetRequiredService<IMapper>();
            var propertiesDecorators = provider.GetRequiredService<IPropertiesDecorators>();

            propertiesDecorators.AddDecorator("UserAttributes", async (data, property, eventName) => {
                var roles = await roleService.GetListForUserIdAsync(((UserAttributes)data).Id);
                if (eventName == "Result")
                    property["roles"] = roles.Select(mapper.Map<Role, RoleResponse>);
                else
                    property["roles"] = roles;
            });
        }
    }
}
