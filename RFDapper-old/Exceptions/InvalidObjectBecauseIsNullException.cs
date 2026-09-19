using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    public class InvalidObjectBecauseIsNullException()
        : HttpException(500, "Invalid object because the object is null.")
    { }
}
