namespace RFOperators
{
    public class And(params Operator[] values)
        : NAry(values)
    {
        public override int Precedence { get; } = 22;
    }
}
