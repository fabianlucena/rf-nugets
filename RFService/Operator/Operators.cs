namespace RFService.Operator
{
    public class DistinctTo(object value) { }

    public class Op
    {
        public static DistinctTo DistinctTo(object value) => new(value);
    }
}
