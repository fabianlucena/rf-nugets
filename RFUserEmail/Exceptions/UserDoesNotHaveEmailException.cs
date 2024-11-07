using RFService.Exceptions;

namespace RFAuth.Exceptions
{
    public class UserDoesNotHaveEmailException() : HttpException(404)
    {
    }
}