using Microsoft.Extensions.DependencyInjection;
using RFAuthEntities.Entities;
using RFAuthIServices.DTO;
using RFAuthServices.DTO;
using RFBaseEntities.Libs;
using RFRGOBACIServices.DTO;
using RFRGOBACIServices.IServices;

namespace RFRGOBACServices
{
    public static class Decorators
    {
        public static async Task<object> LoginResponse(object obj, string name, IServiceProvider? serviceProvider, object? data)
        {
            if (serviceProvider is null || obj is not SessionResponse response || data is not Session session)
                return obj;

            var sessionDataService = serviceProvider.GetRequiredService<ISessionDataService>();
            var sessionData = await sessionDataService.GetSingleOrDefaultBySession(session);
            if (sessionData is null)
                return response;

            var sessionDataResponse = new SessionDataResponse(sessionData);
            if (sessionDataResponse == null)
                return response;

            response.Data ??= new DataDictionary();
            foreach (var item in sessionDataResponse.Data)
                response.Data[item.Key] = item.Value;

            return response;
        }

        public static async Task<object> CheckAutorization(object obj, string name, IServiceProvider? serviceProvider, object? data)
        {
            if (serviceProvider is null || obj is not CachedSession cachedSession || data is not Session session)
                return obj;

            var sessionDataService = serviceProvider.GetRequiredService<ISessionDataService>();
            var sessionData = await sessionDataService.GetSingleOrDefaultBySession(session);
            if (sessionData is null)
                return cachedSession;

            var rolesNames = new HashSet<string>(sessionData.RolesNames ?? [])
            { "user" };

            var PermissionsNames = new HashSet<string>(sessionData.PermissionsNames ?? [])
            { "default" };

            cachedSession.Items["SessionId"] = session.Id;
            cachedSession.Items["UserId"] = session.UserId;
            cachedSession.Items["Session"] = session;
            cachedSession.Items["User"] = session.User;
            cachedSession.Items["Device"] = session.Device;
            cachedSession.Items["RolesId"] = sessionData.RolesId;
            cachedSession.Items["RolesNames"] = rolesNames;
            cachedSession.Items["PermissionsNames"] = PermissionsNames;

            return cachedSession;
        }
    }
}
