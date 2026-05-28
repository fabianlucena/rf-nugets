using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using RFEntities.Entities;
using RFIServices.IServices;

namespace RFAuth
{
    public static class SeedSystemData
    {
        public static async Task Setup(IServiceProvider provider)
        {
            var userTypeService = provider.GetService<IUserTypeService>() ??
                throw new Exception("Can't get IUserTypeService.");

            var userService = provider.GetService<IUserService>() ??
                throw new Exception("Can't get IUserService.");

            var userType = await userTypeService.GetSingleByNameOrCreateAsync(
                "user",
                null,
                async T => new UserType
                {
                    Name = "user",
                    Title = "User",
                    IsTranslatable = true,
                }
            ); 

            var user = await userService.GetSingleByUsernameOrCreateAsync(
                "admin",
                null,
                async T => new User
                {
                    TypeId = userType.Id,
                    Username = "admin",
                    DisplayName = "Administrador",
                }
            );

            var userPasswordService = provider.GetService<IUserPasswordService>() ??
                throw new Exception("Can't get IUserPasswordService.");

            await userPasswordService.CreateIfNotExistsByUsernameAsync("admin", "admin");
        }
    }
}
