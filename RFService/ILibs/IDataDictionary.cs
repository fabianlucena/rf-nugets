using RFService.Libs;

namespace RFService.ILibs
{
    public interface IDataDictionary
        : IDictionary<string, object?>
    {
        public DataDictionary GetPascalized();

        public bool IsNullValue(string key);

        public bool IsNotNullValue(string key);

        public bool TryGetInt64(string key, out long value);

        public bool TryGetNotNullString(string key, out string value);

        public bool TryGetNotNullOrEmptyString(string key, out string value);

        public bool TryGetNotNullStrings(string key, out IEnumerable<string> value);

        public bool TryGetBool(string key, out bool value);

        public bool TryGetGuid(string key, out Guid value);

        public bool TryGetGuids(string key, out IEnumerable<Guid> value);

        public bool TryGetNullableObjects(string key, out IEnumerable<object?> value);

        public T ToObject<T>()
            where T : new();

        public object? GetValue(object? element, bool camelize = false);
    }
}
