using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoEndpointsSectionInOAuth2ProvidersConfigurationException()
    : HttpException(400, "No endpoints section in OAuth2Providers configuration")
{
}
