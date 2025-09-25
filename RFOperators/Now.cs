namespace RFOperators
{
    public class Now()
        : Operator
    {
        public override int Precedence { get; } = 0;

        public override bool HasColumn(string column)
            => false;

        public override bool SetForColumn(string column, object? value)
            => false;
    }
}
