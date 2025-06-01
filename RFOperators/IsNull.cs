namespace RFOperators
{
    public class IsNull
        : Unary
    {
        public override int Precedence { get; } = 18;

        public IsNull(Operator value)
            : base(value)
        { }

        public IsNull(string column)
            : base(column)
        { }
    }
}
