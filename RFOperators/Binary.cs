namespace RFOperators
{
    public class Binary
        : Operator
    {
        public Operator Op1 { get; private set;  }
        public Operator Op2 { get; private set; }

        public Binary(Operator op1, Operator op2)
        {
            Op1 = op1;
            Op2 = op2;
        }

        public Binary(Operator column, object? value)
        {
            Op1 = column;
            Op2 = new Value(value);
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

        public override bool SetForColumn<T>(string column, object? value)
        {
            if (this is T)
            {
                if (Op1 is Column c1 && c1.Name == column)
                {
                    Op2 = new Value(value);
                    return true;
                }
                else if (Op2 is Column c2 && c2.Name == column)
                {
                    Op1 = new Value(value);
                    return true;
                }
            }

            return Op1.SetForColumn<T>(column, value)
                || Op2.SetForColumn<T>(column, value);
        }
    }
}
