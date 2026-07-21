namespace RFDapperDriverPostgreSQL.Exceptions
{
    [Serializable]
    public class InvalidColumnAliasException(string? message)
        : Exception($"Invalid column alias: {message}")
    {
    }
}