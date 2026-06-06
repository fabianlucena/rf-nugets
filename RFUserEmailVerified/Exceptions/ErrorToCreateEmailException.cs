using RFBase.Exceptions;

namespace RFUserEmailVerified.Exceptions
{
    public class ErrorToCreateEmailException()
        : HttpException(404, "Error creating email")
    {
    }
}