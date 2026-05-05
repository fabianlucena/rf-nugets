using Microsoft.Extensions.DependencyInjection;
using RFAuthEntities.Entities;
using RFAuthIServices.IServices;
using RFAuthServices.Decorators;
using RFBaseEntities.Libs;
using RFRGOBACIServices.DTO;
using RFRGOBACIServices.IServices;

namespace RFRGOBACServices.Decorators
{
    public class SessionServiceDecorator(
        ISessionService _sessionService,
        IServiceProvider serviceProvider
    )
        : SessionServiceDecoratorBase(_sessionService),
        ISessionService
    {
        private readonly ISessionService sessionService = _sessionService;

        public override async Task<Session> DecorateAsync(Session session)
        {
            session = await sessionService.DecorateAsync(session);
            session = await DecorateSession(session);
            return session;
        }

        public async Task<Session> DecorateSession(Session session)
        {
            session = new Session(session);

            var sessionDataService = serviceProvider.GetRequiredService<ISessionDataService>();
            var sessionData = await sessionDataService.GetSingleOrDefaultBySession(session);
            if (sessionData is null)
                return session;

            session.Data ??= new DataDictionary();
            session.Data["Organizations"] = sessionData.Organizations;
            session.Data["CurrentOrganization"] = sessionData.CurrentOrganization;

            if (sessionData.GroupIds is not null)
                session.Data["GroupIds"] = sessionData.GroupIds;

            if (sessionData.GroupIds is not null)
                session.Data["GroupNames"] = sessionData.GroupNames;

            if (sessionData.RoleIds is not null)
                session.Data["RoleIds"] = sessionData.RoleIds;

            if (sessionData.RoleNames is not null)
                session.Data["RoleNames"] = sessionData.RoleNames;

            if (sessionData.PermissionNames is not null)
                session.Data["PermissionNames"] = sessionData.PermissionNames;

            var sessionDataResponse = new SessionDataResponse(sessionData);
            if (sessionDataResponse == null)
                return session;

            session.DataResponse ??= [];
            foreach (var item in sessionDataResponse.Data)
                session.DataResponse[item.Key] = item.Value;

            return session;
        }
    }
}
