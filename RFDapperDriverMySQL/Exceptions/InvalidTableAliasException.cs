namespace RFDapperDriverMySQL.Exceptions
{
    [Serializable]
    public class InvalidTableAliasException(string? message)
        : Exception($"Invalid table alias: {message}")
    {
    }
}