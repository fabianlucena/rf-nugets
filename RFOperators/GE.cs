namespace RFOperators
{
    public class GE
        : Binary
    {
        public override int Precedence { get; } = 13;

        public GE(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public GE(string column, object? value)
            : base(column, value)
        { }
    }
}
