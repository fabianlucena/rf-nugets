using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions
{
    public class NoCodeProvidedInDataException()
        : HttpException(400, "No code provided in data.")
    {
    }
}
