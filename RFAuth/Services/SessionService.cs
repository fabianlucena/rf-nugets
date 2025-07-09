using RFAuth.Entities;
using RFAuth.IServices;
using RFService.Services;
using RFAuth.Util;
using RFService.IRepo;
using RFService.Repo;
using RFAuth.Exceptions;
using RFHttpExceptions.Exceptions;
using RFService.Libs;

namespace RFAuth.Services
{
    public class SessionService(IRepo<Session> repo)
        : ServiceTimestampsIdUuid<IRepo<Session>, Session>(repo),
            ISessionService
    {
        public override async Task<Session> ValidateForCreationAsync(Session data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.UserId == 0)
                throw new NullFieldException("UserId");

            if (string.IsNullOrWhiteSpace(data.Token))
                data.Token = Token.GetString(64);

            if (string.IsNullOrWhiteSpace(data.AutoLoginToken))
                data.AutoLoginToken = Token.GetString(64);

            return data;
        }

        public async Task<Session> CreateForUserIdAndDeviceIdAsync(Int64 userId, Int64 deviceId)
        {
            var session = new Session
            {
                UserId = userId,
                DeviceId = deviceId,
                Token = "",
                AutoLoginToken = "",
            };

            return await CreateAsync(session);
        }

        public async Task<Session> CreateForUserAndDeviceAsync(User user, Device device)
        {
            return await CreateForUserIdAndDeviceIdAsync(user.Id, device.Id);
        }

        public async Task<Session> CreateForAutoLoginTokenAndDeviceAsync(string autoLoginToken, Device device)
        {
            var session = await GetSingleAsync(new QueryOptions
            {
                Filters = {
                    { "AutoLoginToken", autoLoginToken }
                }
            });

            if (session.ClosedAt != null)
            {
                throw new SessionClosedException();
            }

            return await CreateForUserIdAndDeviceIdAsync(session.UserId, device.Id);
        }

        public async Task<bool> CloseForIdAsync(Int64 id)
        {
            return (await UpdateAsync(
                new DataDictionary { { "ClosedAt", DateTime.UtcNow } },
                new QueryOptions { Filters = { { "Id", id} } }
            )) > 0;
        }

        public Task<Session?> GetForTokenOrDefaultAsync(string token)
        {
            return repo.GetSingleOrDefaultAsync(new QueryOptions { Filters = { { "Token", token } } });
        }
    }
}
