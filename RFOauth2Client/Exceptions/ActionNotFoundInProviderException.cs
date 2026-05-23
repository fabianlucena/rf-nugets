using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions
{
    public class ActionNotFoundInProviderException(string actionName, string name)
        : HttpException(404, "Action '{0}' not found in provider '{1}'.", actionName, name)
    {
    }
}
