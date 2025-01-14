using System.Data.Common;

namespace RFOperators
{
    public class ST_Contains
        : Binary
    {
        public ST_Contains(Column column1, Column column2)
            : base(column1, column2)
        {
        }

        public ST_Contains(string column1, string column2)
            : base(new Column(column1), new Column(column2))
        {
        }
    }
}
