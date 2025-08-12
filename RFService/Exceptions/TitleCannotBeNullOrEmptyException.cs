using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class TitleCannotBeNullOrEmptyException()
        : HttpException(400, "Title cannot be null or empty.")
    {
    }
}
