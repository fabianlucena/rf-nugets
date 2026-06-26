using Microsoft.Extensions.DependencyInjection;
using RFBase.ILibs;
using RFBase.Libs;
using RFEntities.Entities;
using RFHttpAction.Entities;
using RFHttpAction.Exceptions;
using RFHttpAction.IRepositories;
using RFHttpAction.IServices;
using RFHttpAction.QueryOptions;
using RFIServices.IServices;
using RFRegisterService.Attributes;
using RFServices.Exceptions;
using RFServices.Services;

namespace RFHttpAction.Services;

[RegisterService]
public class HttpActionService(
    IHttpActionRepository httpActionRepository,
    IServiceProvider serviceProvider
)
    : AuditableEntityService<HttpAction>(httpActionRepository, serviceProvider),
        IHttpActionService
{
    public override async Task<long> GetCurrentUserId()
    {
        if (catchedCurrentUserId <= 0)
            catchedCurrentUserId = await UserService.GetCurrentOrSystemUserIdAsync();

        return catchedCurrentUserId;
    }

    public override async Task<HttpAction> ValidateForCreateAsync(HttpAction entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        if (string.IsNullOrEmpty(entity.Token))
            entity.Token = await Token.GetString(64, async token => await GetSingleOrDefaultByTokenAsync(token) == null);

        return entity;
    }

    public async Task<HttpAction?> GetSingleOrDefaultByTokenAsync(string token, HttpActionQueryOptions? options = null)
    {
        options = options?.Clone() ?? new HttpActionQueryOptions();
        options.Token = token;
        return await GetSingleOrDefaultAsync(options);
    }

    public async Task<HttpAction> GetSingleByTokenAsync(string token, HttpActionQueryOptions? options = null)
        => await GetSingleOrDefaultByTokenAsync(token, options)
            ?? throw new NoEntityFoundForTokenException(token);

    public async Task CloseForIdAsync(long id)
    {
        await UpdateByIdAsync(
            id,
            new DataDictionary {
                { "ClosedAt", DateTime.UtcNow },
            }
        );
    }

    public string GetUrl(HttpAction action)
    {
        return "v1/action/" + action.Token;
    }
}
