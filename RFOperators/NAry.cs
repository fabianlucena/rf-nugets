namespace RFOperators
{
    public class NAry(params Operator[] ops)
        : Operator
    {
        public Operator[] Ops { get; } = ops;

        public override bool HasColumn(string column)
            => Ops.Any(op => op.HasColumn(column));

        public override bool SetForColumn<T>(string column, object? value)
        {
            foreach (var filter in Ops)
            {
                var result = filter.SetForColumn<T>(column, value);
                if (result)
                    return true;
            }

            return false;
        }
    }
}
