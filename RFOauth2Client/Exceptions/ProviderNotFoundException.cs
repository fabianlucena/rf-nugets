using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class ProviderNotFoundException(string name)
    : HttpException(404, "Provider '{0}' not found.", name)
{
}
