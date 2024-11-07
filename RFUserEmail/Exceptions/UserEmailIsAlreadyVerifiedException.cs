using RFService.Exceptions;

namespace RFAuth.Exceptions
{
    public class UserEmailIsAlreadyVerifiedException() : HttpException(400)
    {
    }
}