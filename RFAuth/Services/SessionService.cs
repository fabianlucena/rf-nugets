using RFAuth.DTO;
using RFAuth.Entities;
using RFAuth.IRepositories;
using RFAuth.IServices;
using RFAuth.QueryOptions;
using RFBase.ILibs;
using RFBase.Libs;
using RFRegisterService.Attributes;
using RFServices.Services;
using System.Text.Json;

namespace RFAuth.Services;

[RegisterService]
public class SessionService(
    ISessionRepository sessionRepository,
    IServiceProvider serviceProvider
)
    : CreatableEntityService<Session>(sessionRepository, serviceProvider),
    ISessionService
{
    public int TokenSize { get; set; } = 64;

    public override async Task<Session> ValidateForCreateAsync(Session session)
    {
        session = await base.ValidateForCreateAsync(session);

        if (string.IsNullOrEmpty(session.AuthorizationToken))
        {
            session.AuthorizationToken = await Token.GetString(TokenSize, async token => await GetFirstOrDefaultByAuthorizationTokenAsync(token) == null);
        } else if (await GetFirstOrDefaultByAuthorizationTokenAsync(session.AuthorizationToken) != null)
        {
            throw new InvalidOperationException("A session with the same token already exists.");
        }

        if (string.IsNullOrEmpty(session.AutoLoginToken))
        {
            session.AutoLoginToken = await Token.GetString(TokenSize, async token => await GetFirstOrDefaultByAutoLoginTokenAsync(token) == null);
        }
        else if (await GetFirstOrDefaultByAutoLoginTokenAsync(session.AutoLoginToken) != null)
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
        };

        if (data is not null)
            session.Data = data;

        session = await CreateAsync(session);
        session = await GetSingleByIdAsync(session.Id, new SessionQueryOptions
        {
            IncludeUser = true,
            IncludeDevice = true,
        });

        return session;
    }

    public async Task<Session?> GetFirstOrDefaultByAuthorizationTokenAsync(string token, SessionQueryOptions? options = null)
        => await GetFirstOrDefaultAsync(new SessionQueryOptions(options) { AuthorizationToken = token });

    public async Task<Session?> GetSingleOrDefaultByAuthorizationTokenAsync(string token, SessionQueryOptions? options = null)
        => await GetSingleOrDefaultAsync(new SessionQueryOptions(options) { AuthorizationToken = token });

    public async Task UpdateLastUsageAsync(long sessionId)
    {
        await UpdateByIdAsync(sessionId, new DataDictionary { ["LastUsedAt"] = DateTime.UtcNow });
    }

    public async Task AddDataByIdAsync(long sessionId, string key, object value)
    {
        var session = await GetSingleByIdAsync(sessionId);
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
        => await GetFirstOrDefaultAsync(new SessionQueryOptions(options) { AutoLoginToken = autoLoginToken });

    public async Task CloseByIdAsync(long sessionId)
    {
        await UpdateByIdAsync(sessionId, new DataDictionary { ["ClosedAt"] = DateTime.UtcNow });
    }

    public async Task<Session> DecorateAsync(Session session)
        => session;
}
