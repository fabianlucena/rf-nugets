using RFHttpExceptions.Exceptions;
using System.Text.Json;

namespace RFDapper.Exceptions
{
    [Serializable]
    public class UnknownOperationException(string operation)
        : HttpException(500, $"Unknown operation: {JsonSerializer.Serialize(operation)}.")
    {
    }
}