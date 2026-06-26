using RFBase.Libs;
using RFHttpAction.Entities;
using RFHttpAction.Exceptions;
using RFHttpAction.IRepositories;
using RFHttpAction.IServices;
using RFHttpAction.QueryOptions;
using RFRegisterService.Attributes;
using RFServices.Services;

namespace RFHttpAction.Services;

[RegisterService]
public class HttpActionService(
    IHttpActionRepository httpActionRepository,
    IServiceProvider serviceProvider
)
    : CreatableEntityService<HttpAction>(httpActionRepository, serviceProvider),
        IHttpActionService
{
    public override async Task<HttpAction> ValidateForCreateAsync(HttpAction data)
    {
        data = await base.ValidateForCreateAsync(data);

        if (string.IsNullOrEmpty(data.Token))
        {
            data.Token = await Token.GetString(64, async token => await GetSingleOrDefaultByTokenAsync(token) == null);
        }

        return data;
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
            new DataDictionary { { "ClosedAt", DateTime.UtcNow } }
        );
    }

    public string GetUrl(HttpAction action)
    {
        return "v1/action/" + action.Token;
    }
}
