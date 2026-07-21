using RFBase.Exceptions;

namespace RFAuth.Exceptions
{
    public class DeviceNotFoundException()
        : HttpException(401, "Device not found.")
    {
    }
}