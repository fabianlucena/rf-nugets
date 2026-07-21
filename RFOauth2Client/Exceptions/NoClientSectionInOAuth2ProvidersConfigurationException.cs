using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class NoClientSectionInOAuth2ProvidersConfigurationException()
    : HttpException(400, "No client section in OAuth2Providers configuration")
{
}
