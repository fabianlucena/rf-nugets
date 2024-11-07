using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFHttpAction.Entities;
using RFService.IRepo;

namespace RFHttpActionDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFHttpActionDapper(this IServiceCollection services)
        {
            services.AddScoped<Dapper<HttpActionType>, Dapper<HttpActionType>>();
            services.AddScoped<Dapper<HttpAction>, Dapper<HttpAction>>();

            services.AddScoped<IRepo<HttpActionType>, Dapper<HttpActionType>>();
            services.AddScoped<IRepo<HttpAction>, Dapper<HttpAction>>();
        }
    }
}
