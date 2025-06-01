namespace RFOperators
{
    public abstract class Unary
        : Operator
    {
        public Operator Op { get; }

        public Unary(Operator op)
        {
            Op = op;
        }

        public Unary(string column)
        {
            Op = new Column(column);
        }

        public override bool HasColumn(string column)
            => Op.HasColumn(column);

        public override bool SetForColumn<T>(string column, object? value)
            => Op.SetForColumn<T>(column, value);
    }
}
