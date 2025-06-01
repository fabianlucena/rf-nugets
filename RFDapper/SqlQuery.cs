using RFService.Libs;

namespace RFDapper
{
    public class SqlQuery
    {
        public string Sql = "";

        public DataDictionary Data = [];

        public bool IsNull = false;

        public int Precedence = 0;

        public SqlQuery() { }
    }
}
