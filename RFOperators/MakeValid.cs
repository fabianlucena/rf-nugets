namespace RFOperators
{
    public class MakeValid
        : Unary
    {
        public MakeValid(Column column)
            : base(column)
        {
        }

        public MakeValid(string column)
            : base(new Column(column))
        {
        }
    }
}
