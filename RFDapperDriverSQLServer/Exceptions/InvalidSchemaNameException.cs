namespace RFDapperDriverSQLServer.Exceptions
{
    [Serializable]
    public class InvalidSchemaNameException(string? message)
        : Exception($"Invalid schema name: {message}")
    {
    }
}