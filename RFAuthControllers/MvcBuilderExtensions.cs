using Microsoft.Extensions.DependencyInjection;

namespace RFAuthControllers
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddRFAuthControllers(this IMvcBuilder builder)
        {
            builder.AddApplicationPart(typeof(MvcBuilderExtensions).Assembly)
                .AddControllersAsServices();

            return builder;
        }

    }
}
