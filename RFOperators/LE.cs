namespace RFOperators
{
    public class LE
        : Binary
    {
        public override int Precedence { get; } = 13;

        public LE(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public LE(string column, object? value)
            : base(column, value)
        { }
    }
}
