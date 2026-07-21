using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    public class NothingToUpdateException()
        : HttpException(500, $"No data to update.")
    { }
}
