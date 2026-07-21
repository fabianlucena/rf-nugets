using Microsoft.Extensions.DependencyInjection;

namespace RFOauth2ClientControllers;

public static class AddRFOauth2ClientControllersRegistration
{
    public static IMvcBuilder AddRFAuthControllers(this IMvcBuilder builder)
    {
        builder.AddApplicationPart(typeof(AddRFOauth2ClientControllersRegistration).Assembly)
            .AddControllersAsServices();

        return builder;
    }

}
