using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class ARowWithTheTitleAlreadyExistsException(string title)
        : HttpException(400, "A row with the title \"{0}\" already exists.", title)
    {
    }
}
