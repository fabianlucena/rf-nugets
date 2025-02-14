namespace RFOperators
{
    public class Value(object? data)
        : Operator
    {
        public object? Data { get; } = data;

        public override bool HasColumn(string column)
            => false;

        public override bool SetForColumn<T>(string column, object? value)
            => false;
    }
}
