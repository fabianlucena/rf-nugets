namespace RFOperators
{
    public class Eq
        : Binary
    {
        public Eq(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public Eq(string column, object? value)
            : base(column, value)
        { }
    }
}
