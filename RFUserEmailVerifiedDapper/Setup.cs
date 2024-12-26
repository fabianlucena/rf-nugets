using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFUserEmailVerified.Entities;

namespace RFUserEmailVerifiedDapper
{
    public static class Setup
    {
        public static void ConfigureRFUserEmailVerifiedDapper(IServiceProvider services)
        {
            var dapperService = services.GetService<Dapper<UserEmailVerified>>() ??
                throw new Exception($"No service UserEmailVerified");

            dapperService.CreateTable();
        }
    }
}