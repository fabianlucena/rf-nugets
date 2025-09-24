namespace RFDapperDriverPostgreSQL.Exceptions
{
    [Serializable]
    public class InvalidColumnNameException(string? message)
        : Exception($"Invalid column name: {message}")
    {
    }
}