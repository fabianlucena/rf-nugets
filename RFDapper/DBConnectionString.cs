using RFService.Repo;

namespace RFDapper
{
    public class DBConnectionString
    {
        static public string Default { get; set; } = "";

        static public string Get<TEntity>()
        {
            return Default;
        }
    }
}
