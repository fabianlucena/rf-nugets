namespace RFOperators
{
    public class Like
        : Binary
    {
        public override int Precedence { get; } = 18;

        public Like(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public Like(string column, Operator value)
            : base(column, value)
        { }

        public Like(string column, object? value)
            : base(column, value)
        { }
    }
}
