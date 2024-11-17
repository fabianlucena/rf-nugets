using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class TitleCannotBeNullOrEmptyException()
        : HttpException(400)
    {
    }
}
