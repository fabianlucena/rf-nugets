namespace RFOperators
{
    public class NAry(params Operator[] ops)
        : Operator
    {
        public Operator[] Ops { get; } = ops;

        public override bool HasColumn(string column)
            => Ops.Any(op => op.HasColumn(column));
    }
}
