using Microsoft.Extensions.DependencyInjection;

namespace RFAuthControllers;

public static class AddRFAuthControllersRegistration
{
    public static IMvcBuilder AddRFAuthControllers(this IMvcBuilder builder)
    {
        builder.AddApplicationPart(typeof(AddRFAuthControllersRegistration).Assembly)
            .AddControllersAsServices();

        return builder;
    }

}
