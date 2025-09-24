namespace RFDapperDriverPostgreSQL.Exceptions
{
    [Serializable]
    public class UnknownJoinTypeException(string? message)
        : Exception($"Invalid join type: {message}")
    {
    }
}