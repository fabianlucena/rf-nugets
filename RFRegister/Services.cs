using Microsoft.Extensions.DependencyInjection;
using RFRegister.IServices;
using RFRegister.Services;

namespace RFRegister
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFRegister(this IServiceCollection services)
        {
            services.AddScoped<IRegisterService, RegisterService>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}