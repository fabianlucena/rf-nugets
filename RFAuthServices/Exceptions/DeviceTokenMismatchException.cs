using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class DeviceTokenMismatchException()
        : HttpException(401, "Device token mismatch.")
    {
    }
}