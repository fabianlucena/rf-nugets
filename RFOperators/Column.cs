namespace RFOperators
{
    public class Column(string name)
        : Operator
    {
        public string Name { get; } = name;

        public override bool HasColumn(string column)
            => Name == column;
    }
}
