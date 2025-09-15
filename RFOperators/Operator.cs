namespace RFOperators
{
    public abstract class Operator
    {
        public abstract int Precedence { get; }

        public abstract bool HasColumn(string column);

        public abstract bool SetForColumn(string column, object? value);
    }
}
