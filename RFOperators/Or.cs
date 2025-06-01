namespace RFOperators
{
    public class Or(params Operator[] values)
        : NAry(values)
    {
        public override int Precedence { get; } = 24;
    }
}
