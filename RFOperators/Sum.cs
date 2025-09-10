namespace RFOperators
{
    public class Sum
        : Unary
    {
        public override int Precedence { get; } = 1;

        public Sum(Operator value)
            : base(value)
        { }

        public Sum(string column)
            : base(column)
        { }
    }
}
