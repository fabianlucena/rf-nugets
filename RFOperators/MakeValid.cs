namespace RFOperators
{
    public class MakeValid
        : Unary
    {
        public override int Precedence { get; } = 0;

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
