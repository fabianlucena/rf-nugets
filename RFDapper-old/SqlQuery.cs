using RFService.Libs;

namespace RFDapper
{
    public class SqlQuery<T> where T : class, new()
    {
        public string Sql = "";

        public string SqlNoCommand = "";

        public T Data = new();

        public bool IsNull = false;

        public int Precedence = 0;

        public SqlQuery() { }
    }
}
