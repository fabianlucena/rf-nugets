namespace RFOperators
{
    public class NE
        : Binary
    {
        public NE(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public NE(string column, object? value)
            : base(column, value)
        { }
    }
}
