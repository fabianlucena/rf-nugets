using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions
{
    public class ProviderIsNullException()
        : HttpException(404, "Provider cannot be null.")
    {
    }
}
