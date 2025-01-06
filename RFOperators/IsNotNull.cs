namespace RFOperators
{
    public class IsNotNull
        : Unary
    {
        public IsNotNull(Operator value)
            : base(value)
        { }

        public IsNotNull(string column)
            : base(column)
        { }
    }
}
