using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFAuthIRepositories.Repositories;
using RFAuthIServices.IServices;
using RFBaseEntities.ILibs;
using RFBaseEntities.Libs;
using RFBaseServices.Services;
using System.Security.Cryptography;
using System.Text.Json;

namespace RFAuthServices.Services
{
    public class SessionService(ISessionRepository sessionRepository)
        : CreatableEntityService<Session>(sessionRepository),
        ISessionService
    {
        public int TokenSize { get; set; } = 64;

        public override async Task<Session> ValidateForCreateAsync(Session session)
        {
            session = await base.ValidateForCreateAsync(session);

            if (string.IsNullOrEmpty(session.Token))
            {
                int byteCount = (int)Math.Ceiling(TokenSize / 4.0) * 3;
                do
                {
                    byte[] bytes = RandomNumberGenerator.GetBytes(byteCount);
                    var token = Convert.ToBase64String(bytes)[..TokenSize];
                    session.Token = token;
                } while (await GetFirstOrDefaultByTokenAsync(session.Token) != null);
            } else if (await GetFirstOrDefaultByTokenAsync(session.Token) != null)
            {
                throw new InvalidOperationException("A session with the same token already exists.");
            }

            if (string.IsNullOrEmpty(session.AutoLoginToken))
            {
                int byteCount = (int)Math.Ceiling(TokenSize / 4.0) * 3;
                do
                {
                    byte[] bytes = RandomNumberGenerator.GetBytes(byteCount);
                    var token = Convert.ToBase64String(bytes)[..TokenSize];
                    session.AutoLoginToken = token;
                } while (await GetFirstOrDefaultByTokenAsync(session.AutoLoginToken) != null);
            }
            else if (await GetFirstOrDefaultByTokenAsync(session.AutoLoginToken) != null)
            {
                throw new InvalidOperationException("A session with the same auto-login token already exists.");
            }

            session.LastUsedAt = DateTime.UtcNow;

            if (session.ExpireAt <= DateTime.UtcNow)
            {
                session.ExpireAt = DateTime.UtcNow.AddHours(24);
            }

            return session;
        }

        public override async Task<IDataDictionary> ValidateForUpdate(IDataDictionary data)
        {
            data["LastUsedAt"] = DateTime.UtcNow;
            return data;
        }

        public async Task<Session> CreateAsync(long userId, long deviceId, IDataDictionary? data = null)
        {
            var session = new Session
            {
                ExpireAt = DateTime.MinValue,
                AutoLoginToken = string.Empty,
                UserId = userId,
                DeviceId = deviceId,
                CreatedById = userId,
                Data = data,
            };

            session = await CreateAsync(session);
            session = await GetSingleByIdAsync(session.Id, new SessionQueryOptions
            {
                IncludeUser = true,
                IncludeDevice = true,
            });

            return session;
        }
    
        public async Task<Session?> GetFirstOrDefaultByTokenAsync(string token, SessionQueryOptions? options = null)
        {
            return await sessionRepository.GetFirstOrDefaultByTokenAsync(token, options);
        }

        public async Task UpdateLastUsageAsync(long sessionId)
        {
            await UpdateByIdAsync(sessionId, new DataDictionary { ["LastUsedAt"] = DateTime.UtcNow });
        }

        public async Task AddDataByIdAsync(long sessionId, string key, object value)
        {
            var session = await GetSingleByIdAsync(sessionId);
            session.Data ??= new DataDictionary();
            session.Data[key] = value;
            await UpdateByIdAsync(
                sessionId,
                new DataDictionary
                {
                    { "DataJson", JsonSerializer.Serialize(session.Data) }
                }
            );
        }

        public async Task<Session?> GetFirstOrDefaultByAutoLoginTokenAsync(string autoLoginToken, SessionQueryOptions? options = null)
        {
            return await sessionRepository.GetFirstOrDefaultByAutoLoginTokenAsync(autoLoginToken, options);
        }

        public async Task CloseByIdAsync(long sessionId)
        {
            await UpdateByIdAsync(sessionId, new DataDictionary { ["ClosedAt"] = DateTime.UtcNow });
        }
    }
}
