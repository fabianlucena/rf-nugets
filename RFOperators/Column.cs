namespace RFOperators
{
    public class Column(string name)
        : Operator
    {
        public override int Precedence { get; } = 0;

        public string Name { get; } = name;

        public override bool HasColumn(string column)
            => Name == column;

        public override bool SetForColumn<T>(string column, object? value)
            => false;
    }
}
