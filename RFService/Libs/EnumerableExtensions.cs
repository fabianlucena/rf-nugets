namespace RFService.Libs
{
    public static class EnumerableExtensions
    {
        public static DataDictionary ToDataDictionary(
            this IEnumerable<KeyValuePair<string, object?>> source
        )
        {
            var result = new DataDictionary();
            foreach (var pair in source)
                result.Add(pair.Key, pair.Value);

            return result;
        }
    }
}
