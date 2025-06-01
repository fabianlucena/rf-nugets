namespace RFOperators
{
    public class Eq
        : Binary
    {
        public override int Precedence { get; } = 14;

        public Eq(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public Eq(string column, Operator value)
            : base(column, value)
        { }

        public Eq(string column, object? value)
            : base(column, value)
        { }
    }
}
