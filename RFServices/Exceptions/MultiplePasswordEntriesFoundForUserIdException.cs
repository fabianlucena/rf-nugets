using RFBase.Exceptions;

namespace RFServices.Exceptions
{
    public class MultiplePasswordEntriesFoundForUserIdException(long userId)
        : HttpException(500, "Multiple password entries found for user ID {0}.", userId.ToString())
    { }
}
