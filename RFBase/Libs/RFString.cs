namespace RFBase.Libs
{
    public class RFString
    {
        public static string FirstNonEmpty(params string?[] values)
            => values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? "";
    }
}
