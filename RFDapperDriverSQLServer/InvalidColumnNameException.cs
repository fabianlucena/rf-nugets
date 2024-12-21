namespace RFDapperDriverSQLServer
{
    [Serializable]
    public class InvalidColumnNameException(string? message)
        : Exception($"Invalid column name brackets are forbidden: {message}")
    {
    }
}