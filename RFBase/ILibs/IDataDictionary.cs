using RFBase.Libs;

namespace RFBase.ILibs
{
    public interface IDataDictionary
        : IDictionary<string, object?>
    {
        IDataDictionary FilterKeys(params string []keys);

        IDataDictionary GetPascalized();

        bool IsNullValue(string key);

        bool IsNotNullValue(string key);

        bool TryGetInt64(string key, out long value);

        bool TryGetString(string key, out string? value);

        bool TryGetNotNullString(string key, out string value);

        bool TryGetNotNullOrEmptyString(string key, out string value);

        bool TryGetNotNullStrings(string key, out IEnumerable<string> value);

        bool TryGetBool(string key, out bool value);

        bool TryGetGuid(string key, out Guid value);

        bool TryGetGuids(string key, out IEnumerable<Guid> value);

        bool TryGetDecimal(string key, out Decimal value);

        bool TryGetNullableObjects(string key, out IEnumerable<object?> value);

        T ToObject<T>()
            where T : new();

        object? GetValue(object? element, bool camelize = false);

        string GetJson();
    }
}
