namespace RFDapperDriverPostgreSQL.Exceptions
{
    [Serializable]
    public class NoConnectionStringProvidedException()
        : Exception("No connection string provided.")
    {
    }
}