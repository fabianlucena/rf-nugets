namespace RFOperators
{
    public class In
        : Binary
    {
        public override int Precedence { get; } = 18;

        public In(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public In(string column, object? value)
            : base(column, value)
        { }
    }
}
