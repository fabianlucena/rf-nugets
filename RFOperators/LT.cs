namespace RFOperators
{
    public class LT
        : Binary
    {
        public override int Precedence { get; } = 13;

        public LT(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public LT(string column, object? value)
            : base(column, value)
        { }
    }
}
