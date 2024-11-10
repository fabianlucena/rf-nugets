using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RFService.Operator
{
    public class DistinctTo(object? value)
    {
        public object? Value { get; } = value;
    }

    public class Op
    {
        public static DistinctTo DistinctTo(object value) => new(value);
    }
}
