using RFHttpExceptions.Exceptions;

namespace RFService.Exceptions
{
    public class FilterForColumnDoesNosExistException(string name)
        : HttpException(400, "Filter for column \"{0}\" does not exists.", name)
    {
    }
}
