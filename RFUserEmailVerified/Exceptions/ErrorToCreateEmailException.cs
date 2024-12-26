using RFHttpExceptions.Exceptions;

namespace RFUserEmailVerified.Exceptions
{
    public class ErrorToCreateEmailException()
        : HttpException(404, "Error to create email")
    {
    }
}