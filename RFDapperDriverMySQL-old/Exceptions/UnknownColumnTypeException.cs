namespace RFDapperDriverMySQL.Exceptions
{
    [Serializable]
    public class UnknownColumnTypeException(string type)
        : Exception($"Unknown type {type}")
    {
    }
}