using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    [Serializable]
    public class UnknownBinaryOperatorException(string name)
        : HttpException(500, "Unknown binary operation: {0}.", name)
    {
    }
}