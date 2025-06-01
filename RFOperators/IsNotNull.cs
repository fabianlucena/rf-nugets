namespace RFOperators
{
    public class IsNotNull
        : Unary
    {
        public override int Precedence { get; } = 18;

        public IsNotNull(Operator value)
            : base(value)
        { }

        public IsNotNull(string column)
            : base(column)
        { }
    }
}
