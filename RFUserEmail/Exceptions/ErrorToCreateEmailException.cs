using RFHttpExceptions.Exceptions;

namespace RFUserEmail.Exceptions
{
    public class ErrorToCreateEmailException()
        : HttpException(404, "Error to create email")
    {
    }
}