namespace RFOperators
{
    public class Add
        : Binary
    {
        public override int Precedence { get; } = 11;

        public Add(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public Add(Operator column, object? value)
            : base(column, value)
        { }

        public Add(string column, object? value)
            : base(column, value)
        { }
    }
}
