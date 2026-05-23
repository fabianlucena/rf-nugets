using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class DeviceTokenMismatchException()
        : HttpException(401, "Device token mismatch.")
    {
    }
}