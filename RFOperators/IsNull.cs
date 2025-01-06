namespace RFOperators
{
    public class IsNull
        : Unary
    {
        public IsNull(Operator value)
            : base(value)
        { }

        public IsNull(string column)
            : base(column)
        { }
    }
}
