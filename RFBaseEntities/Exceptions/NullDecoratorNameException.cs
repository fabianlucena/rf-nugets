using RFHttpExceptions.Exceptions;

namespace RFBaseEntities.Exceptions
{
    public class NullDecoratorNameException()
        : HttpException(400, "Decorator name parameter cannot be null or empty.")
    { }
}
