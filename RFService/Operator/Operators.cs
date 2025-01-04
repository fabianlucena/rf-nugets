namespace RFService.Operator
{
    public class Column(string value)
    {
        public string Value { get; } = value;
    }

    public class DistinctTo(object value)
    {
        public object Value { get; } = value;
    }

    public class NotNull()
    {
    }

    public class IsNull(object value)
    {
        public object Value { get; } = value;
    }

    public class Eq(object value1, object value2)
    {
        public object Value1 { get; } = value1;
        public object Value2 { get; } = value2;
    }

    public class And(params object[] values)
    {
        public object[] Values { get; } = values;
    }

    public class GE(object value)
    {
        public object Value { get; } = value;
    }

    public class Op
    {
        public static Column Column(string value) => new(value);
        public static DistinctTo DistinctTo(object value) => new(value);
        public static NotNull NotNull() => new();
        public static IsNull IsNull(object value) => new(value);
        public static Eq Eq(object value1, object value2) => new(value1, value2);
        public static And And(params object[] values) => new(values);
        public static GE GE(object value) => new(value);
    }
}
