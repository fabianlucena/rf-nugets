using RFHttpExceptions.Exceptions;

namespace RFDapper.Exceptions
{
    [Serializable]
    public class UnknownNAryOperatorException(string name)
        : HttpException(500, $"Unknown n-ary operation: {name}.")
    {
    }
}