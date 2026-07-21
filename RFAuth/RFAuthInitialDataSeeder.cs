using RFAuth.IServices;
using RFEntities.Entities;
using RFIServices.IServices;
using RFServices.Attributes;
using RFServices.Interfaces;

namespace RFAuth;

[SeedData(true)]
public class RFAuthInitialDataSeeder(
    IUserService userService,
    IUserTypeService userTypeService,
    IUserPasswordService userPasswordService
) : ISeeder
{
    public async Task Run()
    {
        var userType = await userTypeService.GetOrCreateByNameAsync(
            "user",
            createFactory: async T => new UserType
            {
                Name = "user",
                Title = "User",
                IsTranslatable = true,
            }
        ); 

        var user = await userService.GetOrCreateByUsernameAsync(
            "admin",
            createFactory: async T => new User
            {
                TypeId = userType.Id,
                Username = "admin",
                DisplayName = "Administrador",
            }
        );

        await userPasswordService.CreateIfNotExistsByUsernameAsync("admin", "admin");
    }
}
