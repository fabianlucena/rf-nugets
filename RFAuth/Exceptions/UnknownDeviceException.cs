using RFHttpExceptions.Exceptions;

namespace RFAuth.Exceptions
{
    public class UnknownDeviceException()
        : HttpException(401)
    {
    }
}
