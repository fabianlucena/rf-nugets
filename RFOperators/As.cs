namespace RFOperators
{
    public class As(Operator value, string alias)
        : Unary(value)
    {
        public override int Precedence { get; } = 1;

        public string Alias { get; } = alias;
    }
}
