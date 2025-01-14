namespace RFOperators
{
    public class Binary
        : Operator
    {
        public Operator Op1 { get; }
        public Operator Op2 { get; }

        public Binary(Operator op1, Operator op2)
        {
            Op1 = op1;
            Op2 = op2;
        }

        public Binary(string column, Operator value)
        {
            Op1 = new Column(column);
            Op2 = value;
        }

        public Binary(string column, object? value)
        {
            Op1 = new Column(column);
            Op2 = new Value(value);
        }

        public override bool HasColumn(string column)
            => Op1.HasColumn(column) || Op2.HasColumn(column);
    }
}
