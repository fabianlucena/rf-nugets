using Microsoft.Extensions.DependencyInjection;
using RFDapper;
using RFUserEmail.Entities;

namespace RFUserEmailDapper
{
    public static class Setup
    {
        public static void ConfigureRFUserEmailDapper(IServiceProvider services)
        {
            var dapperService = services.GetService<Dapper<UserEmail>>() ??
                throw new Exception($"No service UserEmail");

            dapperService.CreateTable();
        }
    }
}