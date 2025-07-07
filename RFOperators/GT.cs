namespace RFOperators
{
    public class GT
        : Binary
    {
        public override int Precedence { get; } = 13;

        public GT(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public GT(string column, object? value)
            : base(column, value)
        { }
    }
}
