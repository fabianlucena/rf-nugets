namespace RFOperators
{
    public class NotLike
        : Binary
    {
        public NotLike(Operator op1, Operator op2)
            : base(op1, op2)
        { }

        public NotLike(string column, Operator value)
            : base(column, value)
        { }

        public NotLike(string column, object? value)
            : base(column, value)
        { }
    }
}
