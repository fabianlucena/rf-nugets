using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFDBLocalizer.Entities;
using RFLocalizer.Entities;
using RFService.IRepo;

namespace RFDBLocalizerDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFDBLocalizerDapper(this IServiceCollection services)
        {
            services.AddScoped<Dapper<Language>, Dapper<Language>>();
            services.AddScoped<Dapper<Context>, Dapper<Context>>();
            services.AddScoped<Dapper<Source>, Dapper<Source>>();
            services.AddScoped<Dapper<Translation>, Dapper<Translation>>();

            services.AddScoped<IRepo<Language>, Dapper<Language>>();
            services.AddScoped<IRepo<Context>, Dapper<Context>>();
            services.AddScoped<IRepo<Source>, Dapper<Source>>();
            services.AddScoped<IRepo<Translation>, Dapper<Translation>>();
        }
    }
}
