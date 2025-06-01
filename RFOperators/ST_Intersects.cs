using System.Data.Common;

namespace RFOperators
{
    public class ST_Intersects
        : Binary
    {
        public override int Precedence { get; } = 100;

        public ST_Intersects(Operator value1, Operator value2)
            : base(value1, value2)
        {
        }

        public ST_Intersects(string column1, string column2)
            : base(new Column(column1), new Column(column2))
        {
        }
    }
}
