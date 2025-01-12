namespace RFDapperDriverSQLServer.Exceptions
{
    [Serializable]
    public class OrderBySyntaxException(string code)
        : Exception($"Error in order by Syntax: {code}")
    {
    }
}