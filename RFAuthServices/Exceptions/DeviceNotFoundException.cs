using RFHttpExceptions.Exceptions;

namespace RFAuthServices.Exceptions
{
    public class DeviceNotFoundException()
        : HttpException(401, "Device not found.")
    {
    }
}