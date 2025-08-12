using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RFHttpExceptions.Exceptions;
using RFOauth2Client.DTO;
using RFOauth2Client.IServices;
using RFService.Libs;

namespace RFOauth2Client.Controllers
{
    [ApiController]
    [Route("v1/oauth2")]
    public class OAuth2ClientController(
        ILogger<OAuth2ClientController> logger,
        IServiceProvider serviceProvider,
        IMapper mapper
    ) : ControllerBase
    {
        [HttpGet("providers")]
        public async Task<IActionResult> GetProvidersAsync()
        {
            logger.LogInformation("Getting OAuth2 providers.");

            var providerService = serviceProvider.GetRequiredService<IProviderService>();

            var providers = (await providerService.GetListAuthorizeAsync())
                .Select(mapper.Map<AuthorizeProvider, LoginProviderResponse>);

            return Ok(providers);
        }

        [HttpGet("callback/{*route}")]
        public async Task<IActionResult> GetCallbackAsync(
            [FromRoute] string route,
            [FromQuery] string code,
            [FromQuery] string? deviceToken,
            [FromQuery] string? state
        )
        {
            logger.LogInformation("OAuth2 callback received.");

            var parts = route?.Split('/');
            var name = parts?.ElementAtOrDefault(0)
                ?? throw new HttpException(400, $"No provider provided.");

            var actionName = parts?.ElementAtOrDefault(1)
                ?? throw new HttpException(400, $"No action provided.");

            var providerService = serviceProvider.GetRequiredService<IProviderService>();
            var response = await providerService.CallbackAsync(
                name,
                actionName,
                new DataDictionary { 
                    { "deviceToken", deviceToken },
                    { "code", code },
                    { "state", state },
                }
            );

            return Ok(response);
        }
    }
}
