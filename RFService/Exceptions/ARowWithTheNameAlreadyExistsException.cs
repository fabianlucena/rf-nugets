using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class ARowWithTheNameAlreadyExistsException(string name)
        : HttpException(400, $"A row with the name \"{name}\" already exists.")
    {
    }
}
