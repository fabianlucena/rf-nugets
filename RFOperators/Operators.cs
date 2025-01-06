namespace RFOperators
{
    public class Operators
        : List<Operator>
    {
        public Operators()
        { }

        public Operators(Operators ops)
            :base (ops)
        {}

        public void Add(string column, object? value)
            => Add((value is not string && value?.GetType().GetInterface("IEnumerable") != null) ?
                    Op.In(column, value):
                    Op.Eq(column, value)
                );
    }
}
