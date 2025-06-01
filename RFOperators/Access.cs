namespace RFOperators
{
    public class Access
        : Binary
    {
        public override int Precedence { get; } = 4;

        public Access(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public Access(Operator column, object? value)
            : base(column, value)
        { }

        public Access(string column, object? value)
            : base(column, value)
        { }
    }
}
