using RFBase.Exceptions;

namespace RFAuth.Exceptions;

public class ADeviceWithTheSameTokenAlreadyExistsException()
    : HttpException(400, "A device with the same token already exists")
{
}