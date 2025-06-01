namespace RFOperators
{
    public class Not(Operator value)
        : Unary(value)
    {
        public override int Precedence { get; } = 21;
    }
}
