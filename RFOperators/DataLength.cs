namespace RFOperators
{
    public class DataLength
        : Unary
    {
        public override int Precedence { get; } = 1;

        public DataLength(Operator value)
            : base(value)
        { }

        public DataLength(string column)
            : base(column)
        { }
    }
}
