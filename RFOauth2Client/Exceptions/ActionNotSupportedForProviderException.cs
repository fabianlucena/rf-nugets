using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class ActionNotSupportedForProviderException(string actionName, string name)
    : HttpException(400, "Action '{0}' is not supported for provider '{1}'.", actionName, name)
{
}
