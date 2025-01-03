namespace RFDapperDriverSQLServer
{
    [Serializable]
    public class UnknownColumnTypeException(string type)
        : Exception($"Unknown type {type}")
    {
    }
}