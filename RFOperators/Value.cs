namespace RFOperators
{
    public class Value(object? data)
        : Operator
    {
        public override int Precedence { get; } = 0;

        public object? Data { get; } = data;

        public override bool HasColumn(string column)
            => false;

        public override bool SetForColumn(string column, object? value)
            => false;
    }
}
