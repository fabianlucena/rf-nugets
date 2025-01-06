using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    [Serializable]
    public class UnknownUnaryOperatorException(string name)
        : HttpException(500, $"Unknown unary operation: {name}.")
    {
    }
}