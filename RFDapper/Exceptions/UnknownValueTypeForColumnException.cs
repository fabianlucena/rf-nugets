using RFHttpExceptions.Exceptions;
using System.Text.Json;

namespace RFDapper.Exceptions
{
    public class UnknownValueTypeForColumnException(string column, object? value)
        : HttpException(500, $"Unknown value type for column {column}, value {JsonSerializer.Serialize(value)}.")
    { }
}
